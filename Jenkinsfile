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
                        sh 'apt-get update && apt-get install -y libicu-dev'
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
        
        stage('Publish') {
            steps {
                sh 'export PATH="$PATH:$HOME/.dotnet" && dotnet publish src/ColinhoDaCaApi/ColinhoDaCaApi.csproj -c Release -o out'
                echo 'Aplicação publicada ✅'
            }
        }
        
        stage('Docker Build') {
            steps {
                sh 'docker build -t cesarbotas/colinhodaca-api:latest -f deploy/Dockerfile .'
                echo 'Docker image criada ✅'
            }
        }
        
        stage('Docker Push') {
            when {
                branch 'release'
            }
            steps {
                withCredentials([usernamePassword(credentialsId: 'dockerhub',
                                                   usernameVariable: 'DOCKER_USER',
                                                   passwordVariable: 'DOCKER_PASS')]) {
                    sh """
                    docker login -u $DOCKER_USER -p $DOCKER_PASS
                    docker push cesarbotas/colinhodaca-api:latest
                    """
                }
                echo 'Docker image enviada ✅'
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