#!/usr/bin/env bash
#
# Nightly frontend lab performance sweep.
#
# Runs the CI lab runner against the live deployment and drops a Prometheus exposition file
# where node-exporter's textfile collector will serve it. Nothing is pushed and nothing is
# exposed: the box measures itself, exactly like transcendence-postgres-performance-report.
#
# Installed to /root/deploy/web-perf-sweep.sh and invoked by transcendence-web-perf.service.
# See scripts/ops/README.md.
set -Eeuo pipefail

IMAGE="${PERF_IMAGE:-ghcr.io/luisgon-dev/transcendence-perf:main}"
BASE_URL="${PERF_BASE_URL:-https://transcend.kronic.one}"
TEXTFILE_DIR="${PERF_TEXTFILE_DIR:-/var/lib/transcendence-perf/textfile}"
SAMPLES="${PERF_SAMPLES:-3}"
OUT_NAME="web_lab.prom"

log() { printf '%s %s\n' "$(date -Is)" "$*"; }

log "sweep starting: image=${IMAGE} base=${BASE_URL} samples=${SAMPLES}"

install -d -m 0755 "${TEXTFILE_DIR}"

# Pull, but do not fail the run on a transient registry error — a stale image still produces
# usable numbers, whereas skipping the sweep leaves a gap in the series.
if ! docker pull --quiet "${IMAGE}" >/dev/null 2>&1; then
  log "WARN: docker pull failed; using the locally cached image"
  if ! docker image inspect "${IMAGE}" >/dev/null 2>&1; then
    log "ERROR: no local image either, nothing to run"
    exit 1
  fi
fi

# The container writes into a scratch dir we own, then we move the result into place. Two
# reasons: the container runs as a non-root user that will not own the host textfile dir, and
# node-exporter parses whatever it finds, so the file must appear atomically and complete.
SCRATCH="$(mktemp -d)"
trap 'rm -rf "${SCRATCH}"' EXIT
chmod 0777 "${SCRATCH}"

if docker run --rm \
  --network host \
  --shm-size=1g \
  -v "${SCRATCH}:/out" \
  "${IMAGE}" \
  --base-url "${BASE_URL}" \
  --routes scripts/perf/routes.prod.json \
  --samples "${SAMPLES}" \
  --prom-out /out/${OUT_NAME}
then
  if [[ -s "${SCRATCH}/${OUT_NAME}" ]]; then
    install -m 0644 "${SCRATCH}/${OUT_NAME}" "${TEXTFILE_DIR}/${OUT_NAME}.tmp"
    mv -f "${TEXTFILE_DIR}/${OUT_NAME}.tmp" "${TEXTFILE_DIR}/${OUT_NAME}"
    log "sweep complete: $(grep -c '^transcendence_web_lab' "${TEXTFILE_DIR}/${OUT_NAME}") samples published"
  else
    log "ERROR: runner exited 0 but produced no exposition file"
    exit 1
  fi
else
  # Deliberately leave the previous file in place rather than deleting it. The staleness alert
  # on transcendence_web_lab_last_success_unixtime_seconds is what surfaces this; wiping the
  # file would instead make the series vanish, which reads as "no data" rather than "broken".
  log "ERROR: sweep failed; retaining the previous exposition for the staleness alert to catch"
  exit 1
fi
