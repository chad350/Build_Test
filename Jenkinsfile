pipeline {
    agent any

    parameters {
        choice(name: 'PLATFORM', choices: ['mac', 'android'], description: 'Build target platform')
    }

    options {
        timeout(time: 60, unit: 'MINUTES')
        disableConcurrentBuilds()
        skipDefaultCheckout()
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build') {
            steps {
                sh 'rm -rf Builds'
                sh 'chmod +x build.sh'
                sh "./build.sh ${params.PLATFORM}"
            }
        }
    }

    post {
        always {
            archiveArtifacts artifacts: "Logs/build_${params.PLATFORM}.log", allowEmptyArchive: true
        }
        success {
            archiveArtifacts artifacts: 'Builds/**/*.zip, Builds/**/*.apk', fingerprint: true
        }
    }
}
