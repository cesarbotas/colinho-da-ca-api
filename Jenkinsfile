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
                script {
                    try {
                        sh 'dotnet restore src/ColinhoDaCa.sln'
                        echo 'Dependências restauradas ✅'
                    } catch (Exception e) {
                        echo '⚠️ .NET não encontrado - instalando...'
                        sh 'curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0'
                        sh 'export PATH="$PATH:$HOME/.dotnet" && dotnet restore src/ColinhoDaCa.sln'
                        echo 'Dependências restauradas ✅'
                    }
                }
            }
        }
        
        stage('Build') {
            steps {
                sh 'export PATH="$PATH:$HOME/.dotnet" && dotnet build src/ColinhoDaCa.sln --no-restore --configuration Release'
                echo 'Build executado ✅'
            }
        }
        
        stage('Unit Tests') {
            steps {
                sh 'export PATH="$PATH:$HOME/.dotnet" && dotnet test tests/ColinhoDaCa.TestesUnitarios/ColinhoDaCa.TestesUnitarios.csproj --no-build --verbosity normal'
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