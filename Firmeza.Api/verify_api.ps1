$baseUrl = "https://localhost:7237"
$ErrorActionPreference = "Stop"

# Disable SSL certificate validation
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

function Test-Endpoint {
    param($Name, $Method, $Url, $Body, $Token, $ExpectedStatus)
    
    Write-Host "Testing $Name..." -NoNewline
    
    $headers = @{}
    if ($Token) {
        $headers["Authorization"] = "Bearer $Token"
    }
    
    try {
        $params = @{
            Uri = "$baseUrl$Url"
            Method = $Method
            Headers = $headers
            ContentType = "application/json"
        }
        
        if ($Body) {
            $params["Body"] = ($Body | ConvertTo-Json -Depth 10)
        }
        
        $response = Invoke-RestMethod @params -ErrorAction Stop
        
        Write-Host " [OK]" -ForegroundColor Green
        return $response
    }
    catch {
        $status = $_.Exception.Response.StatusCode.value__
        if ($ExpectedStatus -and $status -eq $ExpectedStatus) {
            Write-Host " [OK] (Expected $status)" -ForegroundColor Green
            return $null
        }
        
        Write-Host " [FAILED]" -ForegroundColor Red
        Write-Host "Error: $($_.Exception.Message)"
        if ($_.Exception.Response) {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            Write-Host "Response: $($reader.ReadToEnd())"
        }
        return $null
    }
}

# 1. Test Swagger
try {
    Invoke-RestMethod -Uri "$baseUrl/swagger/v1/swagger.json" | Out-Null
    Write-Host "Swagger UI: [OK]" -ForegroundColor Green
} catch {
    Write-Host "Swagger UI: [FAILED]" -ForegroundColor Red
    Write-Host $_.Exception.Message
}

# 2. Login as Admin
$adminLogin = @{
    username = "admin"
    password = "Admin123!"
}
$adminTokenResponse = Test-Endpoint -Name "Admin Login" -Method "POST" -Url "/api/auth/login" -Body $adminLogin
$adminToken = $adminTokenResponse.token

if (!$adminToken) {
    Write-Host "Failed to get admin token. Aborting."
    exit
}

# 3. Create Product (Admin)
$product = @{
    name = "Test Product"
    description = "A product for testing"
    sku = "TEST-001"
    price = 99.99
    stock = 100
    category = "Test"
    isActive = $true
}
$createdProduct = Test-Endpoint -Name "Create Product (Admin)" -Method "POST" -Url "/api/products" -Body $product -Token $adminToken

# 4. Get Products (Anonymous)
$products = Test-Endpoint -Name "Get Products (Anonymous)" -Method "GET" -Url "/api/products"

# 5. Login as Customer
$customerLogin = @{
    username = "customer"
    password = "Customer123!"
}
$customerTokenResponse = Test-Endpoint -Name "Customer Login" -Method "POST" -Url "/api/auth/login" -Body $customerLogin
$customerToken = $customerTokenResponse.token

# 6. Create Product (Customer) - Should Fail
$product2 = @{
    name = "Customer Product"
    sku = "TEST-002"
    price = 50.00
    stock = 10
}
Test-Endpoint -Name "Create Product (Customer - Should Fail)" -Method "POST" -Url "/api/products" -Body $product2 -Token $customerToken -ExpectedStatus 403

Write-Host "`nVerification Complete!"
