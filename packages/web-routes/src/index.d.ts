/**
 * Collapse a pathname to its bounded route template.
 *
 * The return value is the shared `route` label for both the browser Web Vitals histograms and
 * the `transcendence_web_lab_*` gauges, which is what allows field and lab series for the same
 * route to be charted together. Unrecognised paths collapse to `/_other` so label cardinality
 * stays bounded no matter what gets requested.
 */
export declare function webVitalsRouteTemplate(pathname: string): string;

/** True when `value` is already a canonical route template (i.e. safe to accept as a label). */
export declare function isWebVitalsRouteTemplate(value: string): boolean;
