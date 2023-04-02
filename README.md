## Architecture
### Scope
Read bars from storage and run algos against them. Insert results in storage.

![image](https://user-images.githubusercontent.com/5879344/229288599-a9576e0d-94d4-4e7c-99fa-b374a4deec26.png)


## Tech Debt

 - Bombs between data batches bug
 

## Manual Deployment

_In order to get the latest dockerfile (with all project references) just copy the ports from it, delete it, Add / Docker Support , then paste back the ports._

#### 1. Build a local Docker Image
>_Dockerfile contains some absolute paths to the projects so for this specific project I need to cd inside one dir above the API that holds the Dockerfile resides._
```
cd C:\Coding\repos\quantarcanum\qarc.algo-factory\Qarc.AlgoFactory  
docker build -f C:\Coding\repos\quantarcanum\qarc.algo-factory\Qarc.AlgoFactory\Qarc.AlgoFactory.Adapter.Api\Dockerfile -t qarc-algofactory .

docker images     // check if it's created
```

#### 2. Run a local Docker Container using the Image we've just built
``` 
//docker run -d -p external_port:internal_port image_name
docker run -d -p 3230:80 -p 3231:443 qarc-datafeed

// a few other commands for container manipulation
docker ps -a                                         // list all containers (in order to get the container id)
docker stop container_ID                             // stop container
docker start -i container_id/cointainer_name         // start container
docker exec -it container_ID bash                    // log into a contianer
docker exit                                          // log out of a contianer
docker rm -a container_ID                            // delete a container
```
Check if it works by pinging http://localhost:3230/swagger/index.html

#### 3. Push the Docker Image to Dockerhub
>_On docker hub we have a limitation that it only allows for one private repository. So in order to store multiple images we will use the same image name with different tags. Instead of using the tag to provide the version v1.0.0 we will use the tag as the "image name"._

Login to dockerhub
```
docker login
```
The private repository on docker hub is called: pete3m/quantarcanum and we can push repositories/images to it by running:
``` 
docker push pete3m/quantarcanum:tagname
``` 
The local repository/image on my machine is called: qarc-datafeed
<br>I need to create another repository/image from it, to match the upstream repository name (specifying the image name as tag) 
<br>Using the docker commit command I can create a new image based on an existing container: 
``` 
//docker commit container_ID docker_username/upstream_repository_name:tag_optional
docker commit 2f5cccaedeb7 pete3m/quantarcanum:qarc-datafeed                   // get container_id from step 2
``` 
Now check if created (run "docker images"): you should have an image with: 
- REPOSITORY pete3m/quantarcanum 
- TAG qarc-datafeed

Push the new repository by running: 
``` 
docker push docker_username/image_name:tag

docker push pete3m/quantarcanum:qarc-datafeed
```  
Now you can check [docker hub](https://hub.docker.com/repository/docker/pete3m/quantarcanum/general) 

You can pull a docker image by running:
``` 
docker pull image_name
```  

#### 4. Deploy Service in Kubernetes from dockerhub

Create Kubernetes Deployment, Service & Ingressroute yamls and push to upstream
 - Set Deployment to pull the image from Dockerhub
 - In service.yaml specify ClusterIP and expose 80 and 443
 - In ingressroute create rules for all controllers matching the host and url controller name like so:  
   - match: Host(`qarc.tplinkdns.com`) && Path(`/api/AtasGuerrillaAggregatedData/{*}`)
 - Make sure the url points to kubernetes loadbalancer ip (sudo nano /etc/hosts)

Pull from server and cd into Kubernetes folder where the yamls reside and run
 
 ```
sudo k3s kubectl create -f deployment.yaml
sudo k3s kubectl create -f service.yaml
sudo k3s kubectl create -f ingressroute.yaml
 ```
 *use "delete" instead of "create" if you need to delete the resources first. Or delte them via kubectl commands
 

## CICD

### Prerequisites
1. Generate DockerHub Access Token
1. Export kube.config and make it accessible via web
1. Setup secrets in GitHub

#### 1. Create DockerHub Access Token
Navigate to [https://hub.docker.com/settings/security](https://hub.docker.com/settings/security) and click New Access Token

#### 2. Export baremetal cluster kube.config and make it accessible via web
For this follow [this](https://dev.azure.com/piticasr/Kubernetes/_wiki/wikis/Kubernetes.wiki/198/-1.2-k3s-Install-Expose-Kube.config) page.

#### 3. Setup secrets in GitHub
On Github Navigate to Settings->Secrets and Variables/Actions (github.com/ACCOUNT/REPO/settings/secretes/actions) 
Click on Manage Organization Secrets in quantarcanum case then click on "New Organization Secrets" and create three secrets named as such:

- **Name**: DOCKER_USERNAME 
- **Value**: pete3m
<br><br>
- **Name**: DOCKER_KEY
- **Value**: paste in the access token generated above
<br><br>
- **Name**: KUBE_CONFIG_BASE64
- **Value**: paste in the value generated above

### Github Action yaml
Navigate to the repo's "Actions" tab ([https://github.com/quantarcanum/qarc.datafeed/actions](https://github.com/quantarcanum/qarc.datafeed/actions)) and click on New Workflow and add this yaml code:
```
name: CICD

on:
  push:
    branches: [ "main" ]
  pull_request:
    branches: [ "main" ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      -
        name: Checkout
        uses: actions/checkout@v3
      -
        name: Login to Docker Hub
        uses: docker/login-action@v2
        with:
          username: ${{ secrets.DOCKER_USERNAME }}
          password: ${{ secrets.DOCKER_KEY }}
      -
        name: Set up Docker Buildx
        uses: docker/setup-buildx-action@v2
      -
        name: Build and push
        uses: docker/build-push-action@v4
        with:
          context: ./Qarc.DataFeed
          file: ./Qarc.DataFeed/Qarc.DataFeed.Adapter.Api/Dockerfile
          push: true
          tags: ${{ secrets.DOCKER_USERNAME }}/quantarcanum:qarc-datafeed

      - uses: tale/kubectl-action@v1
        with:
          base64-kube-config: ${{ secrets.KUBE_CONFIG_BASE64 }}
      - run: kubectl delete -f ./Qarc.DataFeed/Kubernetes/deployment.yaml --ignore-not-found=true
      - run: kubectl delete -f ./Qarc.DataFeed/Kubernetes/service.yaml --ignore-not-found=true
      - run: kubectl delete -f ./Qarc.DataFeed/Kubernetes/ingressroute.yaml --ignore-not-found=true
      - run: kubectl create -f ./Qarc.DataFeed/Kubernetes/deployment.yaml
      - run: kubectl create -f ./Qarc.DataFeed/Kubernetes/service.yaml
      - run: kubectl create -f ./Qarc.DataFeed/Kubernetes/ingressroute.yaml
```
**Trigger**: push/PR to main branch<br>

**Checkout**: actions/checkout@v3 action that pulls the code from the main branch<br>

**DockerHub Login**: docker/login-action@v2 uses the github secrets - make sure they match <br>

**Build & Push**: docker/build-push-action@v4 builds the code using the Dockerfile - pay attention to the context - must be one folder up from the dockerfile directory so it finds the other projects referenced in the dockerfile<br>

**Deploy** 
- Here I'm using [this](https://github.com/tale/kubectl-action) action which allows me to run kubectl commands for baremetal cluster.
- First I'm deleting the deployment, service and ingressroute if they exist
- Then I'm recreating them (_I recreate the ingressroute in case I add new controllers to the api_)<br>
- _*TODO: There's probably a more efficient and elegant way of doing this so find it!_
