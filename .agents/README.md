# DarkFactor Agent Kit

This directory contains reusable agent workflows shared by DarkFactor
repositories. It is intentionally independent of application-specific
architecture, build commands, deployment details, and credentials.

## Contents

- `skills/`: on-demand workflows, including scripts, templates, and references.

## Convert To A Submodule

Move this directory into a dedicated `darkfactor-agent-kit` repository, then
replace it in each consuming repository with a Git submodule:

```bash
git rm -r .agents
git commit -m "chore: remove embedded agent kit"
git submodule add <agent-kit-repository-url> .agents
git commit -m "chore: add agent kit submodule"
git submodule update --init --recursive
```

Clone consuming repositories with `git clone --recurse-submodules`, or run
`git submodule update --init --recursive` after cloning.

## Boundaries

- Keep reusable, task-specific workflows in this directory.
- Keep per-repository instructions in the root `AGENTS.md`.
- Do not place credentials, environment files, or application configuration in
  this directory.