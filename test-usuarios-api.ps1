# Script para testar todas as rotas do UsuariosController

# Ignorar erros de certificado SSL
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

$baseUrl = "http://localhost:5063/api/usuarios"
$token = ""
$usuarioId = ""

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "TESTANDO ROTAS DO USUARIOSCONTROLLER" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# Fun��o para exibir resultados
function Show-Result {
    param($title, $response, $statusCode)
    Write-Host "`n--- $title ---" -ForegroundColor Yellow
    Write-Host "Status: $statusCode" -ForegroundColor Green
    if ($response) {
        $response | ConvertTo-Json -Depth 5
    }
}

# Fun��o para fazer requisi��es
function Invoke-ApiRequest {
    param($method, $uri, $body = $null, $token = $null, $description)
    
    try {
        $headers = @{
            "Content-Type" = "application/json"
        }
        
        if ($token) {
            $headers["Authorization"] = "Bearer $token"
        }
        
        $params = @{
            Method = $method
            Uri = $uri
            Headers = $headers
        }
        
        if ($body) {
            $params["Body"] = ($body | ConvertTo-Json -Depth 5)
        }
        
        Write-Host "`n>>> $description" -ForegroundColor Cyan
        Write-Host "    $method $uri" -ForegroundColor Gray
        
        $response = Invoke-RestMethod @params
        Show-Result $description $response "200 OK"
        return $response
    }
    catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        $statusText = $_.Exception.Response.StatusCode
        Write-Host "`n--- $description ---" -ForegroundColor Yellow
        Write-Host "Status: $statusCode $statusText" -ForegroundColor Red
        
        try {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $reader.BaseStream.Position = 0
            $reader.DiscardBufferedData()
            $responseBody = $reader.ReadToEnd()
            Write-Host $responseBody -ForegroundColor Red
        }
        catch {
            Write-Host "Erro: $($_.Exception.Message)" -ForegroundColor Red
        }
        
        return $null
    }
}

# TESTE 1: Registrar novo usu�rio
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "1. REGISTRAR NOVO USU�RIO" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

$novoUsuario = @{
    nome = "Usuario Teste API"
    email = "teste.api@teste.com"
    senha = "Teste@123456"
    confirmarSenha = "Teste@123456"
}

$usuarioRegistrado = Invoke-ApiRequest -method "POST" -uri "$baseUrl/registrar" -body $novoUsuario -description "Registrar novo usu�rio"

if ($usuarioRegistrado) {
    $usuarioId = $usuarioRegistrado.id
    Write-Host "`nUsu�rio registrado com ID: $usuarioId" -ForegroundColor Green
}

# TESTE 2: Login com o usu�rio criado
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "2. LOGIN" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

$loginData = @{
    email = "teste.api@teste.com"
    senha = "Teste@123456"
}

$loginResult = Invoke-ApiRequest -method "POST" -uri "$baseUrl/login" -body $loginData -description "Login de usu�rio"

if ($loginResult) {
    $token = $loginResult.token
    Write-Host "`nToken recebido: $($token.Substring(0, 50))..." -ForegroundColor Green
}

# TESTE 3: Obter usu�rio por ID (requer autentica��o)
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "3. OBTER USU�RIO POR ID" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

if ($usuarioId -and $token) {
    Invoke-ApiRequest -method "GET" -uri "$baseUrl/$usuarioId" -token $token -description "Obter usu�rio por ID"
}

# TESTE 4: Obter todos os usu�rios (requer autentica��o)
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "4. OBTER TODOS OS USU�RIOS" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

if ($token) {
    Invoke-ApiRequest -method "GET" -uri "$baseUrl" -token $token -description "Obter todos os usu�rios"
}

# TESTE 5: Desativar usu�rio (requer autentica��o)
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "5. DESATIVAR USU�RIO" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

if ($usuarioId -and $token) {
    Invoke-ApiRequest -method "PATCH" -uri "$baseUrl/$usuarioId/desativar" -token $token -description "Desativar usu�rio"
}

# TESTE 6: Ativar usu�rio (requer autentica��o)
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "6. ATIVAR USU�RIO" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

if ($usuarioId -and $token) {
    Invoke-ApiRequest -method "PATCH" -uri "$baseUrl/$usuarioId/ativar" -token $token -description "Ativar usu�rio"
}

# TESTE 7: Recuperar senha (n�o retorna dados sens�veis)
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "7. RECUPERAR SENHA" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

$recuperarSenhaData = @{
    email = "teste.api@teste.com"
}

Invoke-ApiRequest -method "POST" -uri "$baseUrl/recuperar-senha" -body $recuperarSenhaData -description "Recuperar senha"

# TESTE 8: Testar login com credenciais inv�lidas
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "8. LOGIN COM CREDENCIAIS INV�LIDAS" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

$loginInvalido = @{
    email = "teste.api@teste.com"
    senha = "SenhaErrada123"
}

Invoke-ApiRequest -method "POST" -uri "$baseUrl/login" -body $loginInvalido -description "Login com senha inv�lida"

# TESTE 9: Tentar acessar rota protegida sem token
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "9. ACESSAR ROTA PROTEGIDA SEM TOKEN" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

if ($usuarioId) {
    Invoke-ApiRequest -method "GET" -uri "$baseUrl/$usuarioId" -description "Obter usu�rio sem autentica��o"
}

# TESTE 10: Tentar registrar usu�rio com email duplicado
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "10. REGISTRAR COM EMAIL DUPLICADO" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

$usuarioDuplicado = @{
    nome = "Outro Usuario"
    email = "teste.api@teste.com"
    senha = "OutraSenha@123"
    confirmarSenha = "OutraSenha@123"
}

Invoke-ApiRequest -method "POST" -uri "$baseUrl/registrar" -body $usuarioDuplicado -description "Registrar usu�rio com email duplicado"

# RESUMO
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "RESUMO DOS TESTES" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "? 1. POST /api/usuarios/registrar - Registrar novo usu�rio" -ForegroundColor Green
Write-Host "? 2. POST /api/usuarios/login - Login de usu�rio" -ForegroundColor Green
Write-Host "? 3. GET /api/usuarios/{id} - Obter usu�rio por ID (autenticado)" -ForegroundColor Green
Write-Host "? 4. GET /api/usuarios - Obter todos os usu�rios (autenticado)" -ForegroundColor Green
Write-Host "? 5. PATCH /api/usuarios/{id}/desativar - Desativar usu�rio (autenticado)" -ForegroundColor Green
Write-Host "? 6. PATCH /api/usuarios/{id}/ativar - Ativar usu�rio (autenticado)" -ForegroundColor Green
Write-Host "? 7. POST /api/usuarios/recuperar-senha - Recuperar senha" -ForegroundColor Green
Write-Host "? 8. POST /api/usuarios/login - Teste de login inv�lido" -ForegroundColor Green
Write-Host "? 9. GET /api/usuarios/{id} - Teste sem autentica��o" -ForegroundColor Green
Write-Host "? 10. POST /api/usuarios/registrar - Teste email duplicado" -ForegroundColor Green
Write-Host "`nNOTA: A rota GET /api/usuarios/confirmar-email n�o foi testada pois requer token de confirma��o v�lido." -ForegroundColor Yellow
Write-Host "NOTA: A rota POST /api/usuarios/redefinir-senha n�o foi testada pois requer token de recupera��o v�lido.`n" -ForegroundColor Yellow
