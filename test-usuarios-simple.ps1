# Script simplificado para testar rotas do UsuariosController

$baseUrl = "http://localhost:5063/api/usuarios"

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "TESTANDO ROTAS DO USUARIOSCONTROLLER" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# TESTE 1: Registrar usu�rio
Write-Host "`n[TESTE 1] POST /api/usuarios/registrar - Registrar novo usu�rio" -ForegroundColor Cyan
try {
    $randomEmail = "teste.api.$(Get-Random)@teste.com"
    $body = @{
        nome = "Usuario Teste API"
        email = $randomEmail
        senha = "Teste@123456"
        confirmarSenha = "Teste@123456"
    } | ConvertTo-Json
    
    Write-Host "Request Body: $body" -ForegroundColor Gray
    
    $response = Invoke-WebRequest -Uri "$baseUrl/registrar" -Method POST -Body $body -ContentType "application/json" -UseBasicParsing
    Write-Host "? Status: $($response.StatusCode)" -ForegroundColor Green
    $result = $response.Content | ConvertFrom-Json
    Write-Host ($result | ConvertTo-Json -Depth 3)
    
    $usuarioId = $result.id
    $email = $result.email
    Write-Host "ID do usu�rio criado: $usuarioId" -ForegroundColor Magenta
} catch {
    Write-Host "? Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $reader.BaseStream.Position = 0
        $responseBody = $reader.ReadToEnd()
        Write-Host "Erro: $responseBody" -ForegroundColor Red
    } else {
        Write-Host "Erro: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# TESTE 2: Login
Write-Host "`n[TESTE 2] POST /api/usuarios/login - Login de usu�rio" -ForegroundColor Cyan
try {
    $body = @{
        email = $email
        senha = "Teste@123456"
    } | ConvertTo-Json
    
    Write-Host "Request Body: $body" -ForegroundColor Gray
    
    $response = Invoke-WebRequest -Uri "$baseUrl/login" -Method POST -Body $body -ContentType "application/json" -UseBasicParsing
    Write-Host "? Status: $($response.StatusCode)" -ForegroundColor Green
    $result = $response.Content | ConvertFrom-Json
    Write-Host "Token recebido: $($result.token.Substring(0, [Math]::Min(50, $result.token.Length)))..." -ForegroundColor Green
    
    $token = $result.token
} catch {
    Write-Host "? Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $reader.BaseStream.Position = 0
        $responseBody = $reader.ReadToEnd()
        Write-Host "Erro: $responseBody" -ForegroundColor Red
    } else {
        Write-Host "Erro: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# TESTE 3: Obter usu�rio por ID (autenticado)
if ($usuarioId -and $token) {
    Write-Host "`n[TESTE 3] GET /api/usuarios/$usuarioId - Obter usu�rio por ID" -ForegroundColor Cyan
    try {
        $headers = @{
            "Authorization" = "Bearer $token"
        }
        
        $response = Invoke-WebRequest -Uri "$baseUrl/$usuarioId" -Method GET -Headers $headers -UseBasicParsing
        Write-Host "? Status: $($response.StatusCode)" -ForegroundColor Green
        $result = $response.Content | ConvertFrom-Json
        Write-Host ($result | ConvertTo-Json -Depth 3)
    } catch {
        Write-Host "? Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
        Write-Host "Erro: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# TESTE 4: Obter todos os usu�rios (autenticado)
if ($token) {
    Write-Host "`n[TESTE 4] GET /api/usuarios - Obter todos os usu�rios" -ForegroundColor Cyan
    try {
        $headers = @{
            "Authorization" = "Bearer $token"
        }
        
        $response = Invoke-WebRequest -Uri "$baseUrl" -Method GET -Headers $headers -UseBasicParsing
        Write-Host "? Status: $($response.StatusCode)" -ForegroundColor Green
        $result = $response.Content | ConvertFrom-Json
        Write-Host "Total de usu�rios: $($result.Count)" -ForegroundColor Magenta
        Write-Host "Primeiros 3 usu�rios:" -ForegroundColor Gray
        Write-Host ($result | Select-Object -First 3 | ConvertTo-Json -Depth 2)
    } catch {
        Write-Host "? Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
        Write-Host "Erro: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# TESTE 5: Desativar usu�rio
if ($usuarioId -and $token) {
    Write-Host "`n[TESTE 5] PATCH /api/usuarios/$usuarioId/desativar - Desativar usu�rio" -ForegroundColor Cyan
    try {
        $headers = @{
            "Authorization" = "Bearer $token"
        }
        
        $response = Invoke-WebRequest -Uri "$baseUrl/$usuarioId/desativar" -Method PATCH -Headers $headers -UseBasicParsing
        Write-Host "? Status: $($response.StatusCode)" -ForegroundColor Green
        Write-Host $response.Content
    } catch {
        Write-Host "? Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
        Write-Host "Erro: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# TESTE 6: Ativar usu�rio
if ($usuarioId -and $token) {
    Write-Host "`n[TESTE 6] PATCH /api/usuarios/$usuarioId/ativar - Ativar usu�rio" -ForegroundColor Cyan
    try {
        $headers = @{
            "Authorization" = "Bearer $token"
        }
        
        $response = Invoke-WebRequest -Uri "$baseUrl/$usuarioId/ativar" -Method PATCH -Headers $headers -UseBasicParsing
        Write-Host "? Status: $($response.StatusCode)" -ForegroundColor Green
        Write-Host $response.Content
    } catch {
        Write-Host "? Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
        Write-Host "Erro: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# TESTE 7: Recuperar senha
if ($email) {
    Write-Host "`n[TESTE 7] POST /api/usuarios/recuperar-senha - Recuperar senha" -ForegroundColor Cyan
    try {
        $body = @{
            email = $email
        } | ConvertTo-Json
        
        $response = Invoke-WebRequest -Uri "$baseUrl/recuperar-senha" -Method POST -Body $body -ContentType "application/json" -UseBasicParsing
        Write-Host "? Status: $($response.StatusCode)" -ForegroundColor Green
        Write-Host $response.Content
    } catch {
        Write-Host "? Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
        Write-Host "Erro: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# TESTE 8: Login com senha inv�lida
if ($email) {
    Write-Host "`n[TESTE 8] POST /api/usuarios/login - Login com senha inv�lida (deve falhar)" -ForegroundColor Cyan
    try {
        $body = @{
            email = $email
            senha = "SenhaErrada123"
        } | ConvertTo-Json
        
        $response = Invoke-WebRequest -Uri "$baseUrl/login" -Method POST -Body $body -ContentType "application/json" -UseBasicParsing
        Write-Host "? Status: $($response.StatusCode) - Deveria ter falhado!" -ForegroundColor Yellow
    } catch {
        Write-Host "? Status: $($_.Exception.Response.StatusCode.value__) (esperado 401)" -ForegroundColor Green
        if ($_.Exception.Response) {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $reader.BaseStream.Position = 0
            $responseBody = $reader.ReadToEnd()
            Write-Host "Mensagem: $responseBody" -ForegroundColor Cyan
        }
    }
}

# TESTE 9: Acessar rota protegida sem token
if ($usuarioId) {
    Write-Host "`n[TESTE 9] GET /api/usuarios/$usuarioId - Sem autentica��o (deve falhar)" -ForegroundColor Cyan
    try {
        $response = Invoke-WebRequest -Uri "$baseUrl/$usuarioId" -Method GET -UseBasicParsing
        Write-Host "? Status: $($response.StatusCode) - Deveria ter falhado!" -ForegroundColor Yellow
    } catch {
        Write-Host "? Status: $($_.Exception.Response.StatusCode.value__) (esperado 401)" -ForegroundColor Green
    }
}

# TESTE 10: Tentar registrar com email duplicado
if ($email) {
    Write-Host "`n[TESTE 10] POST /api/usuarios/registrar - Email duplicado (deve falhar)" -ForegroundColor Cyan
    try {
        $body = @{
            nome = "Outro Usuario"
            email = $email
            senha = "Teste@123456"
            confirmarSenha = "Teste@123456"
        } | ConvertTo-Json
        
        $response = Invoke-WebRequest -Uri "$baseUrl/registrar" -Method POST -Body $body -ContentType "application/json" -UseBasicParsing
        Write-Host "? Status: $($response.StatusCode) - Deveria ter falhado!" -ForegroundColor Yellow
    } catch {
        Write-Host "? Status: $($_.Exception.Response.StatusCode.value__) (esperado 400)" -ForegroundColor Green
        if ($_.Exception.Response) {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $reader.BaseStream.Position = 0
            $responseBody = $reader.ReadToEnd()
            Write-Host "Mensagem: $responseBody" -ForegroundColor Cyan
        }
    }
}

# TESTE 11: Registrar com senhas diferentes (deve falhar)
Write-Host "`n[TESTE 11] POST /api/usuarios/registrar - Senhas diferentes (deve falhar)" -ForegroundColor Cyan
try {
    $body = @{
        nome = "Usuario Teste"
        email = "teste.$(Get-Random)@teste.com"
        senha = "Teste@123456"
        confirmarSenha = "SenhaDiferente@123"
    } | ConvertTo-Json
    
    $response = Invoke-WebRequest -Uri "$baseUrl/registrar" -Method POST -Body $body -ContentType "application/json" -UseBasicParsing
    Write-Host "? Status: $($response.StatusCode) - Deveria ter falhado!" -ForegroundColor Yellow
} catch {
    Write-Host "? Status: $($_.Exception.Response.StatusCode.value__) (esperado 400)" -ForegroundColor Green
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $reader.BaseStream.Position = 0
        $responseBody = $reader.ReadToEnd()
        Write-Host "Mensagem: $responseBody" -ForegroundColor Cyan
    }
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "TESTES CONCLU�DOS" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "RESUMO DAS ROTAS TESTADAS:" -ForegroundColor Yellow
Write-Host "? 1. POST /api/usuarios/registrar - Registrar novo usu�rio" -ForegroundColor White
Write-Host "? 2. POST /api/usuarios/login - Login de usu�rio" -ForegroundColor White
Write-Host "? 3. GET /api/usuarios/{id} - Obter usu�rio por ID (autenticado)" -ForegroundColor White
Write-Host "? 4. GET /api/usuarios - Obter todos os usu�rios (autenticado)" -ForegroundColor White
Write-Host "? 5. PATCH /api/usuarios/{id}/desativar - Desativar usu�rio (autenticado)" -ForegroundColor White
Write-Host "? 6. PATCH /api/usuarios/{id}/ativar - Ativar usu�rio (autenticado)" -ForegroundColor White
Write-Host "? 7. POST /api/usuarios/recuperar-senha - Recuperar senha" -ForegroundColor White
Write-Host "? 8. POST /api/usuarios/login - Teste com senha inv�lida" -ForegroundColor White
Write-Host "? 9. GET /api/usuarios/{id} - Teste sem autentica��o" -ForegroundColor White
Write-Host "? 10. POST /api/usuarios/registrar - Teste email duplicado" -ForegroundColor White
Write-Host "? 11. POST /api/usuarios/registrar - Teste senhas diferentes`n" -ForegroundColor White

Write-Host "NOTA: As rotas GET /api/usuarios/confirmar-email e POST /api/usuarios/redefinir-senha" -ForegroundColor Yellow
Write-Host "      n�o foram testadas pois requerem tokens de confirma��o/recupera��o v�lidos.`n" -ForegroundColor Yellow
