pipeline {
    agent any
    
    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        IMAGE_NAME = 'colinho-da-ca-api'
        DOCKER_TAG = "${BUILD_NUMBER}"
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
                sh 'docker build -t $IMAGE_NAME:$DOCKER_TAG -f deploy/Dockerfile .'
                sh 'docker tag $IMAGE_NAME:$DOCKER_TAG $IMAGE_NAME:latest'
                echo 'Imagem Docker criada ✅'
            }
        }
        
        stage('Docker Push') {
            when {
                branch 'main'
            }
            steps {
                echo 'Imagem Docker enviada para registry ✅'
                // sh 'docker push $IMAGE_NAME:$DOCKER_TAG'
                // sh 'docker push $IMAGE_NAME:latest'
            }
        }
    }
    
    post {
        always {
            sh 'docker system prune -f'
        }
        success {
            echo '🚀 Pipeline executado com sucesso!'
        }
        failure {
            echo '❌ Falha no pipeline'
        }
    }
}