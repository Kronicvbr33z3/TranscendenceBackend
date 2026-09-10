# Nightly frontend lab sweep.
#
# Runs the same runner the CI gate uses (scripts/perf/web-lab.mjs), but against the real
# deployment instead of a seeded stack, and emits Prometheus exposition instead of a pass/fail.
# Both halves are needed: CI measures fixed data so a change is attributable to a commit, this
# measures production so the numbers are true.
#
# Chromium comes from Alpine rather than Puppeteer's downloader — nothing here needs a browser
# download path, and avoiding it keeps the image off `extract-zip` and friends entirely.
FROM node:22-alpine

RUN apk add --no-cache \
      chromium \
      nss \
      freetype \
      harfbuzz \
      ca-certificates \
      ttf-freefont

ENV CHROME_PATH=/usr/bin/chromium-browser \
    NODE_ENV=production

WORKDIR /app

# Pinned to the versions the repo resolves, so the sweep and the gate measure identically.
# This runs BEFORE the workspace package is copied in: npm rewrites node_modules, so a package
# vendored in beforehand gets discarded.
RUN npm install --no-save --omit=dev lighthouse@13.4.1 chrome-launcher@1.2.1

# Only what the runner actually needs: the runner, the shared route vocabulary and the route
# list. No workspace install, no app source.
COPY scripts/perf/web-lab.mjs ./scripts/perf/web-lab.mjs
COPY scripts/perf/routes.prod.json ./scripts/perf/routes.prod.json
COPY packages/web-routes ./node_modules/@transcendence/web-routes

# Chromium must not run as root; Lighthouse also wants a writable HOME for its user-data-dir.
RUN addgroup -S sweep && adduser -S sweep -G sweep \
    && mkdir -p /home/sweep && chown -R sweep:sweep /home/sweep /app
USER sweep
ENV HOME=/home/sweep

ENTRYPOINT ["node", "scripts/perf/web-lab.mjs"]
