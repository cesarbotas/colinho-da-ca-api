pipeline {
    agent any
    
    triggers {
        githubPush()
    }
    
    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        IMAGE_NAME = 'cesarbotas/colinhodaca-api'
        VERSION = "1.0.${BUILD_NUMBER}"
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
        
        stage('Test Coverage') {
            steps {
                sh '''
                export PATH="$PATH:$HOME/.dotnet"
                dotnet test tests/ColinhoDaCa.TestesUnitarios/ColinhoDaCa.TestesUnitarios.csproj \
                    --collect:"XPlat Code Coverage" \
                    --results-directory ./coverage \
                    --verbosity normal
                '''
                
                script {
                    def coverageFile = sh(
                        script: 'find ./coverage -name "coverage.cobertura.xml" | head -1',
                        returnStdout: true
                    ).trim()
                    
                    if (coverageFile) {
                        def coverage = sh(
                            script: "grep -o 'line-rate=\"[0-9.]*\"' ${coverageFile} | head -1 | grep -o '[0-9.]*'",
                            returnStdout: true
                        ).trim()
                        
                        def coveragePercent = (coverage as Double) * 100
                        echo "Cobertura de testes: ${coveragePercent.round(2)}%"
                        
                        if (coveragePercent < 20) {
                            error "Cobertura de testes (${coveragePercent.round(2)}%) está abaixo do mínimo exigido (20%)"
                        }
                        
                        echo 'Cobertura de testes aprovada ✅'
                    } else {
                        echo '⚠️ Arquivo de cobertura não encontrado - continuando...'
                    }
                }
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
                sh """
                docker build -t ${IMAGE_NAME}:${VERSION} -f deploy/Dockerfile .
                docker tag ${IMAGE_NAME}:${VERSION} ${IMAGE_NAME}:latest
                """
                echo "Imagem criada: ${VERSION} ✅"
            }
        }
        
        stage('Docker Push') {
            when {
                branch 'release'
            }
            steps {
                withCredentials([usernamePassword(
                    credentialsId: 'dockerhub',
                    usernameVariable: 'DOCKER_USER',
                    passwordVariable: 'DOCKER_PASS'
                )]) {
                    sh """
                    echo $DOCKER_PASS | docker login -u $DOCKER_USER --password-stdin
                    docker push ${IMAGE_NAME}:${VERSION}
                    docker push ${IMAGE_NAME}:latest
                    """
                }
                echo 'Imagem enviada ao Docker Hub 🚀'
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