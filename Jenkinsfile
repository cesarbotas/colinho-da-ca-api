pipeline {
    agent any
    
    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
    }
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
                echo 'Código fonte baixado ✅'
            }
        }
        
        stage('Restore') {
            steps {
                sh 'dotnet restore src/ColinhoDaCa.sln'
                echo 'Dependências restauradas ✅'
            }
        }
        
        stage('Build') {
            steps {
                sh 'dotnet build src/ColinhoDaCa.sln --no-restore --configuration Release'
                echo 'Build executado ✅'
            }
        }
        
        stage('Unit Tests') {
            steps {
                sh 'dotnet test tests/ColinhoDaCa.TestesUnitarios/ColinhoDaCa.TestesUnitarios.csproj --no-build --verbosity normal --collect:"XPlat Code Coverage"'
                echo 'Testes unitários executados ✅'
            }
        }
        
        stage('Integration Tests') {
            steps {
                sh 'dotnet test tests/ColinhoDaCa.TestesIntegrados/ColinhoDaCa.TestesIntegrados.csproj --verbosity normal'
                echo 'Testes integrados executados ✅'
            }
        }
        
        stage('Publish') {
            steps {
                sh 'dotnet publish src/ColinhoDaCaApi/ColinhoDaCaApi.csproj -c Release -o out'
                echo 'Aplicação publicada ✅'
            }
        }
        
        stage('Docker Build') {
            steps {
                echo 'Docker build desabilitado - Jenkins sem plugin Docker'
            }
        }
        
        stage('Docker Push') {
            when {
                branch 'main'
            }
            steps {
                echo 'Docker push desabilitado - Jenkins sem plugin Docker'
            }
        }
    }
    
    post {
        always {
            echo 'Pipeline finalizado'
        }
        success {
            echo '🚀 Pipeline executado com sucesso!'
        }
        failure {
            echo '❌ Falha no pipeline'
        }
    }
}