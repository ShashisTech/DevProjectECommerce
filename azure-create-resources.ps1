# PowerShell script to create ACR and App Services (simplified). Run with Azure CLI logged in.
param(
  [string]$resourceGroup = "DevProjectECommerce-rg",
  [string]$location = "eastus",
  [string]$acrName = "devprojectecomacr",
  [string]$planName = "devproject-plan"
)

az group create -n $resourceGroup -l $location
az acr create -n $acrName -g $resourceGroup --sku Basic

# Create App Service Plan
az appservice plan create -g $resourceGroup -n $planName --is-linux --sku B1

# Create App Services for each microservice
$services = @("catalogservice","auctionservice","orderservice","transactionservice","userservice","uia")
foreach ($s in $services) {
  az webapp create -g $resourceGroup -p $planName -n $s --deployment-container-image-name "$acrName.azurecr.io/$s:latest"
}

Write-Host "Resources created. Set ACR credentials and configure webapps to pull images if needed."
