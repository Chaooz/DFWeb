# Jira Task Ingest

Quick settings before you use it:

```bash
    export JIRA_BASE_URL="https://your-company.atlassian.net"
    export JIRA_EMAIL="your-email@company.com"
    export JIRA_API_TOKEN="your-api-token"
```

In PowerShell on Windows:

```powershell
    $env:JIRA_BASE_URL = "https://your-company.atlassian.net"
    $env:JIRA_EMAIL = "your-email@company.com"
    $env:JIRA_API_TOKEN = "your-api-token"
```

Run the included scripts from Git Bash or WSL on Windows. They require `curl`
and `jq`.

If running from cloud (GitHub Actions/Codespaces), you can also provide these as environment-backed GitHub secrets:

    GITHUB_SECRET_JIRA_BASE_URL
    GITHUB_SECRET_JIRA_EMAIL
    GITHUB_SECRET_JIRA_API_TOKEN

See [SKILL.md](./SKILL.md) for full usage.
