dotnet sonarscanner begin /k:"danliris-weaving-webapi" /v:"1.0.0" /d:sonar.host.url="http://sonarmoonlay.southeastasia.cloudapp.azure.com" /d:sonar.login="0c718f1c067745516b6fb9f344a983c73c1c6c3d" /d:sonar.exclusions="**/*.js,**/node_modules/**"

dotnet build

dotnet sonarscanner end /d:sonar.login="0c718f1c067745516b6fb9f344a983c73c1c6c3d"