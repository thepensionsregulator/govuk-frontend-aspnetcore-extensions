# Address Lookup Feature Redesign Analysis

**Branches compared:** `v13` (baseline/target) vs `feature/address-lookup-v13-pt2` (beta, not to be merged) — using `git diff v13...feature/address-lookup-v13-pt2` (merge-base `116ea55`).

---

## 1. Executive Summary

**Feature overview:** A GOV.UK-styled address lookup component that lets users search for an address by postcode (via Ordnance Survey's AddressBase Premium/OS Places API), select a result, and populate address fields — with manual UK/international entry fallback and no-JS support.

**Problem statement:** Manually typing a full address is slow and error-prone. TPR needs a reusable, accessible, progressively-enhanced address-lookup component embeddable in both plain ASP.NET Core and Umbraco views, supporting single ("primary") and dual ("primary + secondary/same-as") address scenarios.

**Expected outcome:** A production-grade `tpr-address-lookup` tag helper + JS module that: renders standard GOV.UK inputs by default (works without JS/API), progressively enhances into a postcode-search → select → confirm flow when JS is available, supports UK and international manual entry, and maps OS API/DPA results into `TprAddress` model fields with full validation.

**High-level architecture impact:**
- New client-side module (`wwwroot/tpr/address-lookup/*.js`) — API service, mapper, state machine, validator, component builder — replacing the previous "fieldset only" server-rendered concept.
- New domain model `ITprAddress`/`TprAddress` and `ITprCountryRepository`/`TprAddressLookupEndpointUrlProvider` services in `ThePensionsRegulator.Frontend`.
- Tag helper redesigned: `fieldset` → wrapping `div` (to allow JS to swap fieldset content), new `role` (Primary/Secondary), hint tag helper, `same-as-primary` checkbox.
- Configuration-driven endpoints (`AddressLookup:SearchEndpoint`, `AddressLookup:AddressByIdEndpoint`) and CSP `connect-src` updated to allow `https://api.os.uk`.

**Already in `v13`:** A first-generation, server-side-only address lookup: `tpr-address-lookup` tag helper rendering a `<fieldset>` + legend (with "legend as page heading" option), `TprAddressLookupValidationMetadataProvider` for Umbraco custom error messages, Umbraco block/settings content types, and example pages using a flat `AddressLookupViewModel` (individual Shipping/Billing string properties, no shared address model).

**Added by `feature/address-lookup-v13-pt2`:** The entire JS-driven progressive enhancement layer (search/select/confirm/manual-entry state machine, OS API integration, postcode normalisation/sanitisation, country dictionary service, `ITprAddress`/`TprAddress` shared model with `AddressLine3`, primary/secondary "same as" checkbox behaviour, hint support, `described-by`/`aria-describedby` wiring, example JSON data + 4 new example scenarios, and ~514 lines of new documentation). Also bundles unrelated, pre-existing feature work not yet on `v13` (see §6 "Rebuild Recommendations" — flagged as out of scope).

---

## 2. Feature Inventory

| # | Feature | Purpose | User Value | Current Status | Key Components | Dependencies | Risk |
|---|---------|---------|-------------|--------|-----------------|---------------|------|
| 1 | Static fieldset/legend rendering | Baseline accessible grouping of address inputs | Accessible structure without JS | **Already in v13** | `ComponentGenerator.TprAddressLookup.cs`, `TprAddressLookupTagHelper.cs`, `TprAddressLookupLegendTagHelper.cs` | GOV.UK Frontend CSS | Low |
| 2 | Umbraco custom error messages | Editors can override validation text per block instance | Content flexibility | **Already in v13** | `AddressLookupValidationMetadataProvider.cs` | Umbraco block settings | Low |
| 3 | Wrapping `div` + role attribute | Enables JS to progressively replace static fields | Enables JS enhancement without full page reload | **Added in pt2** | `ComponentGenerator.TprAddressLookup.cs`, `AddressLookupRole.cs` | none | Medium |
| 4 | Hint tag helper | Adds accessible hint text (`aria-describedby`) | Improves guidance/accessibility | **Added in pt2** | `TprAddressLookupHintTagHelper.cs`, `TprAddressLookupContext.cs` | none | Low |
| 5 | Primary/Secondary + "same as" checkbox | Supports paired addresses (e.g. billing/shipping) that can be copied | Reduces re-entry effort | **Added in pt2** | `TprAddressLookupTagHelper.cs`, `component-builder.js` | Feature 3 | Medium |
| 6 | Postcode search (OS API) | Search addresses by postcode/building | Fast, accurate address entry | **Added in pt2** | `api-service.js`, `TprAddressLookupEndpointUrlProvider.cs`, `ITprAddressLookupEndpointUrlProvider.cs` | OS Places API, config, CSP | High (external API) |
| 7 | Postcode normalisation/sanitisation | Normalise spaced/unspaced UK postcodes before searching | Reduces failed searches due to formatting | **Added in pt2** (normaliser latest commit `d3ccdc8c`) | `postcode-normaliser.js`, `postcode-sanitiser.js` | Feature 6 | Low |
| 8 | Address select & confirm state machine | Guided multi-step UX (search→select→confirm) | Clear progressive UX, fewer errors | **Added in pt2** | `state-machine.js`, `index.js` | Feature 6 | Medium |
| 9 | Manual UK / international entry | Fallback when address not found or non-UK | Ensures all users can complete the journey | **Added in pt2** | `index.js`, `field-defaults.js`, `input-builder.js` | Feature 8 | Medium |
| 10 | Country dictionary service | Populate country `<select>`, map to `CountryId` | Structured country data instead of free text | **Added in pt2** | `ITprCountryRepository.cs`, `CountryRepositoryForExampleApp.cs` | Example app only — no production implementation | Medium (needs real data source) |
| 11 | Shared `ITprAddress`/`TprAddress` model | Single canonical address DTO w/ validation attributes, `AddressLine3` | Consistent model across MVC + Umbraco | **Added in pt2** | `Models/ITprAddress.cs`, `Models/TprAddress.cs`, `UmbracoTprAddress.cs` | Replaces v13's flat ViewModel fields | Medium (breaking model change) |
| 12 | Model state population helpers | Populate `ModelState` from a `TprAddress` for prefilled/redisplay scenarios | Correct redisplay of validation + pre-population | **Added in pt2** | `TprAddressModelStateExtensions.cs` (both `Frontend` and `Frontend.Umbraco`) | Feature 11 | Low |
| 13 | UPRN hidden field & DPA mapping | Persist UPRN reference from OS API result | Enables round-trip / re-lookup by ID | **Added in pt2** | `mapper.js`, `TprAddress.UPRNReference` | Feature 6 | Low |
| 14 | No-JS example journeys (JavaScriptDisabled, NoDataProvided, PrimaryDataProvided, PrimaryAndSecondaryDataProvided) | Demonstrates all supported states for both example apps | Test/demo coverage, developer guidance | **Added in pt2** | New controllers/views/viewmodels in both example apps | Feature 11 | Low |
| 15 | CSP `connect-src` for OS API | Allow browser to call `api.os.uk` | Required for API-mode to function | **Added in pt2** | `SecurityHeadersMiddleware.cs` (both apps) | Feature 6 | Medium (security review needed) |
| 16 | JS unit test suite for address lookup | Confidence in client logic | Quality/regression safety | **Added in pt2** | `__tests__/address-lookup/*.tests.js` (8 files, ~1,750 LOC) | Jest | Low |
| 17 | Server-side model tests | Confidence in `TprAddress`/model-state extensions | Quality/regression safety | **Added in pt2** | `TprAddressTests.cs`, `TprAddressModelStateExtensionsTests.cs` (×2 projects) | xUnit | Low |
| 18 | Documentation | Onboarding/reference | Developer enablement | **Added in pt2** | `docs/components/tpr-address-lookup.md` (514 lines) | — | Low |

---

## 3. Technical Analysis

### 3.1 Component Rendering & Structure

**Behaviour:** Renders the address form group and (optionally) enhances it with JS.

**Existing in v13:** `GenerateTprAddressLookup(bool isLegendPageHeading, ...)` renders a bare `<fieldset class="govuk-fieldset tpr-address-lookup">`, with an option for the legend to be an `<h1>` ("page heading" pattern). No hint, no role, no describedby.

**Additional in pt2:** Root element changed to `<div class="tpr-address-lookup" data-address-lookup-role="...">` wrapping an inner `<fieldset class="govuk-fieldset">`. This is a structural/breaking change — the "legend is page heading" option was **deliberately removed** ("Remove support for legend is page heading as this makes the JS too complicated for now" — commit `80044cda`), a hint block was added, and `aria-describedby` wiring introduced. A `same-as-primary` checkbox is rendered for `Secondary` role components.

**Production considerations:** Confirm removal of the page-heading legend pattern is acceptable for all current/planned consumers (may be a regression for pages that used it). The `div` wrapper is required for the JS enhancement to work — this should be treated as the new baseline markup contract, not an optional add-on.

**Risks:** Breaking markup change for any page already using v13's fieldset-only rendering or the page-heading legend option (Medium/High depending on current usage — must audit Umbraco content already using the block).

### 3.2 Address Model

**Behaviour:** Represents a postal address with validation.

**Existing in v13:** No shared model — `AddressLookupViewModel` in the example app has flat `Shipping*`/`Billing*` string properties (no `AddressLine3`, no `CountryId`, string-based country).

**Additional in pt2:** Introduces `ITprAddress`/`TprAddress` (with `AddressLine3`, `CountryId` as `int?`, `UPRNReference`, `PopulateAddress()`), plus an Umbraco-specific `UmbracoTprAddress`. `AddressLookupViewModel` now composes `TprAddress` objects rather than flat strings.

**Production considerations:** This is a breaking data-shape change. Any existing v13 consumer of the flat model will need migration. `CountryId` as `int` implies a fixed country reference dataset — needs a real (non-example) `ITprCountryRepository` implementation (e.g., backed by ISO-3166 list or Umbraco data) before production use.

**Risks:** Data migration for any live content already using the old flat structure; no production `ITprCountryRepository` exists yet — only an example implementation.

### 3.3 Postcode Search / OS API Integration

**Behaviour:** Calls a configurable search endpoint and address-by-id endpoint, filters by building name, normalises/sanitises postcodes before searching.

**Existing in v13:** None — no client-side lookup capability at all.

**Additional in pt2:** `api-service.js` (search + get-by-id, with an "example mode" fallback to static JSON when endpoint ends in `.json`), `TprAddressLookupEndpointUrlProvider` reading `AddressLookup:SearchEndpoint`/`AddressLookup:AddressByIdEndpoint` from configuration, postcode normaliser (spaced/unspaced equivalence — most recent commit in the branch) and sanitiser.

**Production considerations:** The example endpoints point at static JSON files under `wwwroot/AddressLookupData/`. Production needs a real backend proxy/controller to call the OS Places/AddressBase API (API key must not be exposed client-side — currently the design assumes a server-side proxy endpoint URL is injected, which is good, but no actual proxy implementation exists yet, only the example JSON substitute). CSP `connect-src` was widened to `https://api.os.uk` in the example app — for true production use, calls should go through TPR's own backend, and this direct external CSP entry may not be needed/should be reviewed.

**Risks:** No production OS API proxy/controller currently exists (must be built) — High risk if timelines assume this is "already done." API key/secret management and rate-limiting are unaddressed. Building-name filtering is done client-side after fetching all postcode results (fine for typical postcode result-set sizes, but worth confirming with real API volumes).

### 3.4 State Machine / Manual Entry / UX Flow

**Behaviour:** search → select → confirmed, with manual UK/international entry paths, using a small state machine to gate valid transitions.

**Existing in v13:** None.

**Additional in pt2:** `state-machine.js`, `index.js` (614 lines — the main orchestrator), `field-defaults.js`, `input-builder.js`, `component-builder.js`, `mapper.js`, `validator.js`, `utils.js`, `config.js`.

**Production considerations:** `index.js` at 614 lines is a large monolith orchestrating DOM manipulation, event wiring, and state transitions — a strong candidate for modularisation before production hardening (see Rebuild Recommendations). Behaviour should be validated against real screen-reader/keyboard users given the dynamic DOM swapping (fieldset content replaced client-side).

**Risks:** Complexity/maintainability of `index.js`; accessibility of dynamically-injected content (focus management, live region announcements) needs explicit verification — not evidenced by the diff alone (inferred risk, not confirmed by code review of ARIA live regions).

### 3.5 Validation & Error Messaging

**Existing in v13:** `AddressLookupValidationMetadataProvider` supports Umbraco block-setting-driven custom error messages, matched via **exact equality** (`propertyValue.Equals(attribute.ErrorMessage)`).

**Additional in pt2:** Changed matching to `Contains` ("Rename country error message keys for consistency", "Address lookup v13 updates #860") — a behavioural change to substring matching rather than exact match, likely to support dynamic/parameterised messages (e.g., messages combined with field names). Also adds `TprAddressModelStateExtensions.SetModelValues` to redisplay pre-populated/prefilled addresses through `ModelState` (needed for server-rendered redisplay after JS selection posts back, and for the new example scenarios with pre-filled addresses).

**Production considerations:** Confirm the switch from exact-match to `Contains` doesn't create false-positive matches when one configured message is a substring of another (edge case risk).

### 3.6 Configuration & Security

**Additional in pt2:** New `AddressLookup` config section (`SearchEndpoint`, `AddressByIdEndpoint`); CSP `connect-src` updated in both example apps' `SecurityHeadersMiddleware` to include `https://api.os.uk`; `img-src`/`frame-src` for YouTube hardened to explicit domains (this appears bundled from the unrelated YouTube-embed feature, not address lookup itself).

**Security implications:** Any production CSP change must be scoped to the actual OS API/backend proxy domain used in production (not the example app's placeholder). If a server-side proxy pattern is used (recommended), `connect-src` may not need the external OS domain at all, reducing attack surface.

### 3.7 Testing

**Additional in pt2 only:** Comprehensive new Jest suite (8 files, ~1,750 LOC) covering api-service, component-builder, mapper, postcode normaliser/sanitiser/validation, submit-on-click behaviour, and validator; plus xUnit tests for `TprAddress` and `TprAddressModelStateExtensions` in both `ThePensionsRegulator.Frontend.Tests` and `ThePensionsRegulator.Frontend.Umbraco.Tests`. v13 has no equivalent JS tests for address lookup (none existed to enhance).

---

## 4. Feature Gap Analysis

### Already Delivered in v13 (no further implementation needed)
- Base `tpr-address-lookup` tag helper existence and Umbraco block/content-type registration (`tpraddresslookup.config`, `tpraddresslookupsettings.config`).
- `AddressLookupValidationMetadataProvider` (exact-match version) — needs the `Contains` behaviour change (see "Requires Modification"), not a rebuild.
- Fieldset/legend rendering fundamentals (will be re-shaped, not re-invented).

### Requires Modification
- `ComponentGenerator.TprAddressLookup` — must be redesigned from fieldset-only to div+fieldset+hint+role wrapper (breaking markup change).
- `AddressLookupViewModel` / address data shape — migrate from flat strings to shared `ITprAddress`/`TprAddress`.
- `AddressLookupValidationMetadataProvider` matching logic — exact-match → substring/contains (verify no regressions).
- Umbraco content types (`tpraddresslookup.config`, `tpraddresslookupsettings.config`) — significant property additions (hint, role, error messages, address-line-3, country).
- CSP configuration — needs production-appropriate values, not the beta's example-app settings.

### New Capability from `feature/address-lookup-v13-pt2` (must be implemented in new branch)
- Entire client-side JS module set (api-service, mapper, state machine, validator, config, field-defaults, input-builder, postcode normaliser/sanitiser, component-builder, index.js orchestrator).
- `ITprAddress`/`TprAddress`/`UmbracoTprAddress` models and `TprAddressModelStateExtensions`.
- `ITprCountryRepository` interface + **a real production implementation** (only example implementation exists).
- `ITprAddressLookupEndpointUrlProvider` + configuration section.
- Primary/Secondary role + "same as primary" checkbox behaviour.
- Hint tag helper (`TprAddressLookupHintTagHelper`) and `aria-describedby` wiring.
- Postcode normalisation for spaced/unspaced equivalence.
- Documentation (`docs/components/tpr-address-lookup.md`), adapted for the actual production design decisions.
- Full test suites (JS + xUnit) — must be recreated for the redesigned implementation (not copy-pasted, since implementation details will change).

### Optional Enhancements (not required for first production release)
- Expanding the Slice 2 GOV.UK JS component-builder library beyond text input/select/fieldset/button into a fuller component kit (e.g. checkboxes, radios, error summaries, character count) — valuable long-term but out of scope for the address-lookup rebuild itself (developer-feedback item, deliberately narrowed to avoid scope creep — see §5).
- Building-name secondary filtering refinement (`SUB_BUILDING_NAME` matching, added in beta commit `d6580c1d`).
- Multiple demo/example scenario pages (`NoDataProvided`, `PrimaryDataProvided`, etc.) — valuable for dev/QA but not a production runtime requirement.
- Title-casing of OS API result data (cosmetic).

---

## 5. Rebuild Recommendations

**Reuse as-is or with light adaptation:**
- `ITprAddress`/`TprAddress` model shape (well-structured, validated) — good foundation, keep.
- Postcode normaliser/sanitiser and validator logic — small, well-tested, low risk, reuse.
- `TprAddressModelStateExtensions` pattern for redisplay — reuse concept.
- Test suite structure/approach (Jest + xUnit split) — reuse as a template, regenerate tests around the redesigned implementation.

**Should be redesigned rather than lifted wholesale:**
- `index.js` (614 lines) — should be decomposed into smaller, independently testable modules (e.g., separate DOM-rendering, event-binding, and orchestration concerns) before being called production quality. This is the single largest technical-debt item in the beta.
- `ITprCountryRepository` — only an example/static implementation exists; production needs a real data source (config, DB, or Umbraco dictionary-backed) designed from scratch.
- Address search backend — currently only a static-JSON "example mode"; a genuine server-side proxy to the OS Places/AddressBase API (with key management, timeouts, and error handling) must be designed and built, not adapted from the beta.
- CSP/security settings — beta values are for the example app only; production CSP must be designed against the real hosting/API topology (recommend a server-side proxy rather than direct browser calls to `api.os.uk`).

**Technical debt introduced in beta:**
- Monolithic `index.js` orchestrator mixing multiple concerns.
- No production country data source.
- No production address-search backend/proxy — only example JSON substitutes.
- Removal of "legend as page heading" support without an evident replacement path for existing consumers of that pattern.
- Large number of unrelated commits/features merged into the same branch (see below), making the true feature diff harder to isolate — a lesson for how the new production branch should be managed (keep it scoped to address lookup only).

**⚠️ Bundled unrelated work detected in `pt2` (not part of address lookup, must be extracted separately):** The diff also includes changes to `GovUkBreadcrumbComponent`/async breadcrumb support, `OverridableBlockGridModel`/`OverridableBlockListModel` (Umbraco block/value-converter refactor), `IOverridablePublishedElementExtensions`, `tpr-table-csv-download.js` fix, touch-target accessibility fixes, `TprHeaderMenu`/mobile menu changes, and TPR navigational links component/docs. Verifying against `v13`'s own history shows these files' most recent `v13` commits **predate** the corresponding `pt2` commits, confirming they are **not yet in `v13`** and are unrelated feature branches that got merged into `pt2` incidentally (likely via repeated `merge branch 'v13' into pt2` operations picking up sibling feature branches, or `pt2` being used as an integration/staging branch). **Recommendation: do not include these in the address-lookup rebuild plan — track and land them via their own dedicated branches/PRs.**

**Simplification opportunities:**
- Consolidate `field-defaults.js`/`input-builder.js`/`component-builder.js` if their responsibilities overlap (needs closer review during rebuild, flagged as an assumption here since full JS content wasn't exhaustively traced line-by-line).
- Absorb "role" concept into existing tag-helper attribute patterns already used elsewhere in the codebase for consistency (e.g., patterns used by other TPR tag helpers), rather than introducing a bespoke enum only for this component if a shared convention exists.

**Developer feedback incorporated (original feature author, @HarryAmmon):** *"When developing the JS part of the address lookup component, one of the problems was there was no way to build base components like fieldsets, buttons and inputs that used GOV.UK styling. I would have thought a 'slice' I could deliver was being able to create components using JS agnostic of the address lookup work."*

This was reviewed against the actual code and is well-founded, not just anecdotal: `AddressLookupComponentBuilder` (`component-builder.js`) already contains three genuinely generic DOM builders — `createGovukTextInput`, `createGovukSelect`, and `createFieldset` — which apply only standard GOV.UK Design System classes (`govuk-form-group`, `govuk-label`, `govuk-fieldset`, `govuk-input`, `govuk-select`, etc.) with no address-lookup-specific behaviour. These three methods are called **17 times** across `index.js` for the postcode-search fieldset, the UK manual-entry fieldset, and the international manual-entry fieldset alike — i.e. they are already doing generic work, just packaged inside an address-lookup-namespaced class (`AddressLookupComponentBuilder`) and constructed with an address-lookup `config` object. Only `createAddressLookupButton`/`createFindAddressButton`/`createConfirmAddressButton` (and the address-lookup-specific labels/data-attributes they're parameterised with) are genuinely feature-specific. `InputBuilder` (`input-builder.js`) — the fluent `data-val-*` validation-attribute builder — is likewise already framework-agnostic and not address-lookup-specific at all.

**Weighted assessment:** This is a legitimate, low-risk, high-leverage extraction opportunity and has been added as **Slice 2** below (ahead of the address-lookup-specific JS work), since address lookup itself depends on it and it can be built/tested/reused independently of any OS API/search concerns. However, it should be **scoped narrowly** to what's proven reusable today (text input, select, fieldset, and a generic button primitive) rather than expanding into a full GOV.UK JS component-kit covering every Design System component up front — that broader ambition is valuable but represents scope creep for the address-lookup rebuild specifically, and is called out separately as an optional follow-on enhancement rather than a blocking dependency.

---

## 6. Thin Slice Implementation Plan

### Slice 1 — Shared Address Model & Redesigned Markup (Foundation)
**Objective:** Replace v13's flat address fields with the shared `ITprAddress`/`TprAddress` model and redesigned div+fieldset+hint markup, with no JS enhancement yet (server-rendered only).
**Scope:** Add `ITprAddress`, `TprAddress`, `TprAddressModelStateExtensions`; redesign `ComponentGenerator.TprAddressLookup`, `TprAddressLookupTagHelper`, add `TprAddressLookupHintTagHelper`; update Umbraco content types for address-line-3, hint, role attributes.
**Existing Capability:** Builds directly on v13's existing fieldset/legend tag helper and Umbraco block/content-type registration.
**Dependencies:** None (first slice).
**Acceptance Criteria:** Component renders correctly with div+fieldset+hint markup in both example apps with no JS; all existing address fields (incl. AddressLine3) validate and redisplay correctly; Umbraco editors can set hint/role/custom error messages.
**Testing:** Unit tests for `TprAddress` validation and `TprAddressModelStateExtensions` (xUnit); integration test rendering the tag helper via example app; manual check of Umbraco block settings screen and no-JS page render.
**Rollback:** Revert to v13 tag helper/model via feature flag or branch revert; no data persisted yet, so no migration rollback needed.
**Complexity:** Medium.

### Slice 2 — Reusable GOV.UK JS Component Builder Library *(added following developer feedback from the original feature author)*
**Objective:** Extract a standalone, address-lookup-agnostic JS module for building basic GOV.UK-styled DOM elements (form group + label + text input, form group + label + select, fieldset + legend, and a generic button), so that address lookup — and any future JS-driven GOV.UK component — can compose UI from a shared, tested primitive library instead of each feature reinventing DOM construction.
**Scope:** New module (e.g. `wwwroot/tpr/govuk-components/component-builder.js`) containing generic `createTextInput`, `createSelect`, `createFieldset`, and `createButton` builders parameterised by plain arguments (label, name/id, width, options) rather than an address-lookup `config` object; keep the existing `InputBuilder` fluent validation-attribute helper (`input-builder.js`) as-is since it is already generic. Address-lookup-specific concerns (data-attributes, button labels, address-lookup config wiring) stay behind in the address-lookup module and consume this library rather than owning DOM-construction logic themselves.
**Existing Capability:** None in `v13` — this doesn't exist as a distinct capability today. In the beta it exists only *implicitly*, entangled inside `AddressLookupComponentBuilder` (`component-builder.js`): `createGovukTextInput`, `createGovukSelect`, and `createFieldset` are already fully generic (no address-lookup-specific logic) and are called 17 times across `index.js` for the postcode-search, UK manual-entry, and international manual-entry fieldsets alike — confirming the reusable capability is real, just not yet extracted.
**Dependencies:** None (can be built and unit-tested entirely standalone, ahead of or in parallel with Slice 1).
**Acceptance Criteria:** Library can construct a labelled GOV.UK text input, select, fieldset, and button with correct classes/attributes with zero knowledge of address lookup; existing address-lookup JS (once rebuilt in Slice 5) consumes the library rather than duplicating DOM-building logic; library has no dependency on `ADDRESS_LOOKUP_CONFIG` or any address-lookup data attributes.
**Testing:** Jest unit tests per builder (adapt the existing `createGovukTextInput`/`createGovukSelect`/`createFieldset` test cases from `component-builder.tests.js`, generalised to remove address-lookup-specific assertions); manual visual check that output markup matches GOV.UK Design System examples (labels, hint linkage, error state classes).
**Rollback:** Pure library addition with no consumers yet — trivially removable; zero runtime risk until Slice 5 wires it in.
**Complexity:** Low.
> **Scope note:** Deliberately kept narrow — text input, select, fieldset, and button primitives only (the ones proven reusable by actual beta usage), not a full GOV.UK JS component kit covering every Design System component. Expanding further (e.g. checkboxes, radios, error summaries as JS builders) is listed as an **Optional Enhancement**, not a blocking dependency, to avoid scope creep on the address-lookup rebuild.

### Slice 3 — Postcode Normalisation & Validation Utilities
**Objective:** Add postcode normalising/sanitising and client-side validation as standalone, framework-agnostic JS utilities (no search/API yet).
**Scope:** `postcode-normaliser.js`, `postcode-sanitiser.js`, `validator.js`, `utils.js`, `config.js`.
**Existing Capability:** None directly in v13 — purely additive, low-risk client utilities that don't change existing markup/behaviour.
**Dependencies:** Slice 1 (data-attributes on inputs for validator to target).
**Acceptance Criteria:** Utilities pass unit tests matching GDS postcode format examples; no regression to existing no-JS form submission.
**Testing:** Jest unit tests (reuse beta's `postcode-normaliser.tests.js`, `postcode-sanitiser.tests.js`, `postcode-validation.tests.js`, `validator.tests.js` as a starting template); manual test with spaced/unspaced postcodes.
**Rollback:** Remove script include; no server-side impact.
**Complexity:** Low.

### Slice 4 — Server-Side Address Search Proxy (Risk Reduction)
**Objective:** Build a genuine backend proxy endpoint for OS Places/AddressBase API search and get-by-id, replacing the beta's static-JSON-only example.
**Scope:** New controller/endpoint implementing `ITprAddressLookupEndpointUrlProvider` target URLs server-side; secure API key storage; timeout/error handling; configuration section (`AddressLookup:SearchEndpoint`, `AddressLookup:AddressByIdEndpoint`) pointing at the new proxy, not the external API directly.
**Existing Capability:** None — this is the highest-risk net-new capability; tackled early to de-risk before UI work depends on it.
**Dependencies:** Slice 1 (address model to map results into).
**Acceptance Criteria:** Proxy returns DPA-shaped results for a real postcode; API key never exposed to the browser; graceful error responses (rate limit, no results, upstream failure) with correct HTTP status codes.
**Testing:** Integration tests against a mocked OS API (or recorded fixtures); manual smoke test against OS sandbox/test credentials if available; load/error-path tests (timeout, 4xx/5xx from upstream).
**Rollback:** Feature-flag the search endpoint; fallback to manual-entry-only mode if proxy unavailable.
**Complexity:** High.

### Slice 5 — Country Repository (Production Data Source)
**Objective:** Implement a real `ITprCountryRepository` (replacing the example static implementation) backed by an appropriate data source (config file, DB table, or Umbraco dictionary).
**Scope:** `ITprCountryRepository` production implementation; DI registration; country `<select>` population in views.
**Existing Capability:** Interface concept only exists in beta; no production source — new implementation required.
**Dependencies:** Slice 1.
**Acceptance Criteria:** Country select renders full ISO country list with correct default (UK); selected `CountryId` round-trips correctly through model binding.
**Testing:** Unit test repository returns expected dictionary; integration test on view rendering; manual check of select in browser.
**Rollback:** Fallback to a hardcoded minimal list behind a flag if data source unavailable.
**Complexity:** Low–Medium.

### Slice 6 — Progressive Enhancement: Search → Select → Confirm Flow
**Objective:** Deliver the core JS-driven state machine enabling postcode search, result selection, and confirmation, layered on top of Slices 1–4.
**Scope:** `state-machine.js`, `api-service.js` (client), `mapper.js`, decomposed `index.js` (split into smaller modules per Rebuild Recommendations), address-lookup-specific wrapper around the Slice 2 component-builder library, `field-defaults.js`, `input-builder.js`.
**Existing Capability:** None in v13; built on the redesigned markup from Slice 1, the reusable component primitives from Slice 2, and the real backend from Slice 4.
**Dependencies:** Slices 1, 2, 3, 4.
**Acceptance Criteria:** With JS enabled, user can search a real postcode, select a result, and see confirmed address populate the underlying (still-present) form fields; without JS, form still fully functions (progressive enhancement contract preserved); DOM construction goes through the Slice 2 library rather than duplicating it.
**Testing:** Jest unit tests per module (reuse/rewrite beta's `api-service.tests.js`, `mapper.tests.js`); E2E test (e.g. Playwright/Selenium if used elsewhere in repo) covering the happy path; manual accessibility testing (screen reader, keyboard-only) of the dynamic DOM swap.
**Rollback:** Feature flag to disable JS enhancement script inclusion, reverting to Slice 1's static form.
**Complexity:** High.

### Slice 7 — Manual UK / International Entry Fallback
**Objective:** Add manual entry paths when no result is found or address is non-UK.
**Scope:** Manual entry states in state machine; UK/international field-defaults; country-based branching.
**Existing Capability:** Builds on Slices 5 (country data) and 6 (state machine, Slice 2 component library).
**Dependencies:** Slices 5, 6.
**Acceptance Criteria:** User can bypass search and manually complete UK or international address; validation reflects UK vs international rules (e.g., postcode vs postal code labels).
**Testing:** Unit tests for manual-entry state transitions; manual validation of both UK and non-UK paths; accessibility check of dynamically shown/hidden fields.
**Rollback:** Disable manual-entry transition, forcing search-only mode temporarily via config.
**Complexity:** Medium.

### Slice 8 — Primary/Secondary Roles & "Same as Primary" Checkbox
**Objective:** Support paired address components (e.g., correspondence + registered address) with a copy-checkbox.
**Scope:** `AddressLookupRole` enum, role attribute, same-as-primary checkbox rendering/behaviour.
**Existing Capability:** Builds on Slice 1's markup and Slice 6's JS.
**Dependencies:** Slices 1, 6.
**Acceptance Criteria:** Checking "same as primary" copies/mirrors primary address into secondary; unchecking reveals editable secondary fields; validates correctly when secondary diverges.
**Testing:** Unit tests for checkbox behaviour (reuse beta's approach); integration test with two components on one page; manual test of copy/edit/uncheck flow.
**Rollback:** Only render `Primary` role components until this slice is validated; feature-gate `Secondary` role usage.
**Complexity:** Medium.

### Slice 9 — Documentation, Examples & Operational Readiness
**Objective:** Finalise documentation and demo/example pages for developer and content-editor enablement; add monitoring/logging.
**Scope:** `docs/components/tpr-address-lookup.md` (adapted to final design) plus new `docs/components/` entry for the Slice 2 component-builder library, demo pages (`NoDataProvided`, `PrimaryDataProvided`, etc.), logging around the search proxy (Slice 4), basic metrics (search success/failure rate).
**Existing Capability:** Beta's docs/example pages as a structural template only — content must reflect the actual production design decisions from Slices 1–8.
**Dependencies:** Slices 1–8 substantially complete.
**Acceptance Criteria:** Docs accurately describe the shipped component API (both address lookup and the reusable component-builder library); example pages demonstrate all supported states; proxy emits structured logs/metrics for search calls.
**Testing:** Manual doc review against implementation; smoke test of each example page; verify logs appear in the standard TPR logging pipeline.
**Rollback:** N/A (documentation/observability, non-breaking).
**Complexity:** Low.

---

## 7. Delivery Roadmap

| Slice | Description | Existing v13 Dependency | Value Delivered | Complexity |
|-------|-------------|--------------------------|------------------|------------|
| 1 | Shared address model + redesigned markup (no JS) | v13 tag helper, Umbraco block config, validation metadata provider | Foundation model/markup; AddressLine3 & hint support | Medium |
| 2 | Reusable GOV.UK JS component builder library *(developer feedback)* | None (net-new, address-lookup-agnostic) | Reusable text input/select/fieldset/button builders for any future JS component, not just address lookup | Low |
| 3 | Postcode normalisation/validation utilities | None (additive) | Fewer failed searches due to formatting; reusable validation | Low |
| 4 | Server-side address search proxy | None (net-new) | De-risks core external dependency early | High |
| 5 | Production country repository | None (net-new) | Removes reliance on example data | Low–Medium |
| 6 | Search → select → confirm JS flow | Slice 1 markup, Slice 2 component library | Core progressive-enhancement UX delivered | High |
| 7 | Manual UK/international entry | Slices 5, 6 | Handles no-match/non-UK cases | Medium |
| 8 | Primary/Secondary "same as" roles | Slices 1, 6 | Supports paired-address use cases | Medium |
| 9 | Docs, examples, observability | Slices 1–8 | Enablement, support-readiness | Low |

---

## 8. Production Readiness Checklist

- **Security:** API keys for OS Places API stored server-side only (never in client JS); CSP `connect-src` scoped to the actual production proxy domain, not a blanket external API allowance; input sanitisation on postcode/building search terms before proxying.
- **Performance:** Debounce/guard rapid repeat searches; proxy-level caching or rate-limiting to control OS API call volume/cost; assess JS bundle size impact of new `wwwroot/tpr/address-lookup/*` modules.
- **Accessibility:** Verify focus management and screen-reader announcements when DOM content is dynamically swapped (search results, confirmation view); verify manual keyboard-only flow through search/select/confirm/manual-entry; confirm hint `aria-describedby` wiring works with dynamically added/removed fieldset content.
- **Feature flags:** Gate JS-enhanced search behind a flag so it can be disabled independently of markup changes (falls back cleanly to static fields, per Slice 5 rollback).
- **Monitoring:** Track search success rate, no-results rate, and proxy error rate for the OS API integration.
- **Logging:** Structured logs on the search proxy for failed/slow upstream calls; avoid logging full addresses/PII in plain logs (log outcome/status, not full result payloads) — consistent with beta's own principle ("Don't log implementation details").
- **Alerting:** Alert on elevated proxy error rate or OS API quota/rate-limit responses.
- **Documentation:** Component usage doc (`docs/components/tpr-address-lookup.md`) kept current with final API/markup; migration notes for any v13 consumers of the old flat model or page-heading legend option.
- **Operational support:** Runbook for OS API outage (manual-entry fallback should remain fully functional); process for rotating/renewing the OS API key.
- **Testing coverage:** Full Jest suite for all JS modules (reuse beta's suite as a template, rewritten against the redesigned/decomposed modules); xUnit coverage for model, validation metadata, and model-state extensions; integration/E2E coverage of the full search→select→confirm→submit journey, and the no-JS fallback journey.
- **Rollback planning:** Each slice independently flaggable/revertible per its rollback strategy above; database/content migration plan for any Umbraco content already using the v13 flat-field pattern or page-heading legend option before those are removed.

---

**Key assumptions called out:**
1. The unrelated bundled changes (breadcrumb async, block model refactor, table CSV fix, touch-target a11y, navigational links) are treated as **out of scope** for this feature — inferred from the fact `v13`'s file history predates their `pt2` counterparts, indicating they haven't landed on `v13` yet and are incidental to this branch's history rather than part of the address-lookup feature.
2. "Production considerations" around OS API proxying, country data source, and CSP are **inferred intended behaviour** — the beta only ships example/static substitutes for these, so the actual production design for these pieces doesn't yet exist and must be specified/built new.
3. Impact of removing "legend as page heading" support on existing `v13` consumers is inferred as a risk, not confirmed against actual current usage in TPR's live Umbraco content — recommend an audit before Slice 1 ships.
4. Slice 2 (reusable GOV.UK JS component builder) was added based on direct feedback from the original feature author, verified against the actual beta code (`component-builder.js`, `index.js` call sites) rather than accepted at face value. The scope has been deliberately narrowed to the primitives with proven reuse (text input, select, fieldset, button) to balance the developer's valid architectural concern against the risk of scope creep on the address-lookup rebuild itself.
