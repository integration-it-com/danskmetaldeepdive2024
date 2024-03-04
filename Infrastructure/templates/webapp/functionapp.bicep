targetScope = 'resourceGroup'

param appName string
param location string
param env string
param storageAccountConnectionString string
param instrumentationKey string
param farmId string

//Specific for integration platform
param baseStorageAccountName string
param baseStorageAccountConnectionString string

//Runtime
param postFix string = 'dada'
param alwaysOn bool = true

var version = '~4'
var runtime  = 'dotnet-isolated'


resource functionApp 'Microsoft.Web/sites@2021-02-01' = {
  name: '${appName}-${env}-func'
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
    
  }
  properties: {
    serverFarmId: farmId
    siteConfig: {
      alwaysOn: alwaysOn
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storageAccountConnectionString
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: storageAccountConnectionString
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: instrumentationKey
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: runtime
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: version
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: '${appName}${postFix}'
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
        {
          name: '${baseStorageAccountConnectionString}__blobServiceUri'
          value: 'https://${baseStorageAccountName}.blob.core.windows.net'
        }
        {
          name: '${baseStorageAccountConnectionString}__queueServiceUri'
          value: 'https://${baseStorageAccountName}.queue.core.windows.net'
        }
       
      ]
    }
  }
}

output systemPrincipalId string = functionApp.identity.principalId
