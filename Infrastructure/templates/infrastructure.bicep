targetScope = 'resourceGroup'

param appName string
param location string = resourceGroup().location
param env string


//Monitor
module monitor 'monitor/monitor.bicep' = {
  name: 'monitor'
  params: {
    appName: appName
    env: env
    location: location
  }
}

//User Managed Identity
module identity 'identities/userManagedIdentity.bicep' = {
  name: 'identity'
  params: {
    appName: appName
    env: env
    location: location
  }
}



//Storage Account
module storageAccount 'storageAccount/storageAccount.bicep' = {
  name: 'storageAccount'
  params: {
    appName: appName
    location: location
    env: env
  }
}

//Storage Account Container: input
var inputContainerName = 'input'
module container_msgbox 'storageAccount/container.bicep' = {
  name: 'container_msgbox'
  params: {
    containerName: inputContainerName
    storageAccountName: storageAccount.outputs.name
  }
}


//Storage Account Container: output
var outputContainerName = 'output'
module container_archive 'storageAccount/container.bicep' = {
  name: 'container_archive'
  params: {
    containerName: outputContainerName
    storageAccountName: storageAccount.outputs.name
  }
}

//Storage Account Container: config
var configContainerName = 'config'
module container_schemas 'storageAccount/container.bicep' = {
  name: 'container_schemas'
  params: {
    containerName: configContainerName
    storageAccountName: storageAccount.outputs.name
  }
}

//Function app

///App Service Plan
module appServicePlan 'webapp/appServicePlan.bicep' = {
  name: 'appServicePlan'
  params: {
    appName: appName
    env: env
    location: location
  }
}

//Function App Storage Account
module functionAppStorageAccount 'storageAccount/storageAccount.bicep' = {
  name: 'functionAppAccount'
  params: {
    appName: '${appName}fc'
    env: env
    location: location
  }
}

//Function app
module functionApp 'webapp/functionapp.bicep' = {
  name: 'functionApp'
  params: {
    appName: appName
    env: env
    farmId: appServicePlan.outputs.id
    instrumentationKey: monitor.outputs.instrumentationKey
    location: location
    storageAccountConnectionString: functionAppStorageAccount.outputs.connectionString
    baseStorageAccountName: storageAccount.outputs.name
    baseStorageAccountConnectionString: storageAccount.outputs.connectionString
  }
}



//Give MI Role Assignments on Resource Group Level

//Storage Blob Data Owner
//b7e6dc6d-f1e8-4753-8033-0f276bb0955b
module role_blobowner 'rbac/roleAssignment.bicep' = {
  dependsOn:[
    identity
  ]
  name: 'functionAppBlobDataRA'
  params: {
    identityId: functionApp.outputs.systemPrincipalId
    roleName: 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
  }
}


//Storage Queue Data Contributor
//974c5e8b-45b9-4653-ba55-5f855dd0fb88
module role_storagequeuecon 'rbac/roleAssignment.bicep' = {
  dependsOn:[
    identity
  ]
  name: 'functionAppQueueContRA'
  params: {
    identityId: functionApp.outputs.systemPrincipalId
    roleName: '974c5e8b-45b9-4653-ba55-5f855dd0fb88'
  }
}
