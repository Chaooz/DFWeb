---
maxParallelToolCalls: 20
---

# Project Agent Configuration: DarkFactor Web

## Purpose

DarkFactor Web is a .NET 9 front-end and back-end application for publishing
DarkFactor gaming company articles and news.

## Architecture

- Treat controllers as the boundary for all data received from clients.
- Keep the existing Controller -> Provider -> Repository separation. Controllers
  coordinate HTTP concerns, providers own application behavior, and repositories
  own database access.
- Use constructor dependency injection, async/await for I/O-bound work, and
  standard .NET naming and error-handling practices.
- Calls to the game server require authentication unless the API explicitly
  permits anonymous access. Do not log credentials, tokens, or session data.
- Front-end configuration is in `DFWeb.FR.BootStrap/Config`; backend
  configuration models are in `DFWeb.BE/ConfigModel`.

## Build and Test

- Build the application with `dotnet build
  DFWeb.FR.BootStrap/DFWeb.FR.BootStrap.csproj`.
- Run back-end tests with `dotnet test DFWeb.BE.Tests/DFWeb.BE.Tests.csproj`.
- Run front-end tests with `dotnet test DFWeb.FR.Tests/DFWeb.FR.Tests.csproj`.
- Use the VS Code `build_bootstrap`, `rebuild_bootstrap`, `publish_bootstrap`,
  and `watch_bootstrap` tasks when they fit the work.
- Validate containers with `docker compose up --build` when Docker behavior is
  affected.

## Working Practices

- Keep changes focused and preserve existing project conventions.
- Add or update focused tests for changed behavior; run the applicable test
  project before finishing.
- Update documentation and configuration when a change affects either.
- Shared task workflows are supplied by the `.agents` agent-kit dependency.
  Keep project-specific instructions in this file, not in that dependency.
- Create branches and commits only when the user explicitly requests them or
  when repository automation requires them. Before a pull request, run the
  applicable unit tests and validate Docker containers when Docker behavior is
  affected.

## Related Repository

- [DFCommonLib](https://github.com/DarkFactorAS/DFCommonLib.git)
