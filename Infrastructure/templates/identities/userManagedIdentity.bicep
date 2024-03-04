targetScope = 'resourceGroup'

param appName string
param env string
param location string

resource mi 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: '${appName}-${env}-mid'
  location: location
}

output id string = mi.id
output principalId string = mi.properties.principalId
output clientId string = mi.properties.clientId
