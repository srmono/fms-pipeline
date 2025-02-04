pipeline {
    agent any 

    environment {
        DOCKER_IMAGE = 'ustapi/fms-dotnet-app'
        DB_HOST = ''
        DB_PORT = '3306'
        DB_NAME = 'fleetdb'
        DB_USER = 'fleetuser'
        DB_PASSWORD = 'fleetpassword'
    }

    stages {
        stage ('Clone Repository') {
            steps {
                git url: 'https://github.com/srmono/fms-pipeline.git', branch: 'master'
            }
        }
        stage ('Build docker image') {
            steps {
                sh 'docker build -t $DOCKER_IMAGE .'
            }
        }
        stage ("Log in to docker hub") {
            steps {
                withDockerRegistry([credentialsId: 'docker-hub-credentials', url:'https://index.docker.io/v1']) {
                    sh 'echo "Logged into docker hub successfully"'
                }
            }
        }
        stage ("Push Image to  docker hub") {
            steps {
                withDockerRegistry([credentialsId: 'docker-hub-credentials', url:'https://index.docker.io/v1']) {
                    sh 'docker push $DOCKER_IMAGE'
                }
            }
        }
    }
}