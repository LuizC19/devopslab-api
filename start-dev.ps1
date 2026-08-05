Write-Host ""
Write-Host "====================================="
Write-Host "  Iniciando ambiente DevOpsLab"
Write-Host "====================================="
Write-Host ""

Write-Host "Iniciando SQL Server..."
docker compose up -d

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "Erro ao iniciar o Docker Compose."
    exit 1
}

Write-Host ""
Write-Host "Aguardando SQL Server iniciar..."

$maxTentativas = 30
$porta = 1433

for ($i = 1; $i -le $maxTentativas; $i++) {

    $conexao = Test-NetConnection -ComputerName localhost -Port $porta -WarningAction SilentlyContinue

    if ($conexao.TcpTestSucceeded) {
        Write-Host "SQL Server disponível!"
        break
    }

    Write-Host "Tentativa $i/$maxTentativas..."
    Start-Sleep -Seconds 2
}

if (-not $conexao.TcpTestSucceeded) {
    Write-Host ""
    Write-Host "SQL Server não iniciou a tempo."
    exit 1
}

Write-Host ""
Write-Host "Executando a API..."
Write-Host ""

dotnet run