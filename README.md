# Platform Service

This project consists of two services **Platform Service (net7)** and **Command Service (net8)**. Platform service uses PostgreSQL for storage and Command service uses InMemoryDB. **RabbitMQ** is used for asynchronous message passing between two services. **Kubernetes** is used for orchestration of services, database and message broker.

## Environment Setup (Linux/arch)
* Install dotnet sdk for linux [as shown here](https://learn.microsoft.com/en-us/dotnet/core/install/linux-scripted-manual#scripted-install) (We need both .net 7 and .net 8 sdks).
* Install docker with `sudo pacman -S docker`.
* Since we're installing only docker (not docker desktop), we need to separately install docker buildx with `sudo pacman -S docker-buildx`.
* We're using kubeadm to manage kubernetes clusters. Install kubeadm [as shown here](https://kubernetes.io/docs/setup/production-environment/tools/kubeadm/install-kubeadm/). Follow 'Without a package manager' guide since pacman is not supported.
* After kubeadm is configured, we need a Pod network add-on. We're using [**Calico**](https://www.tigera.io/project-calico/). As shown [here](https://docs.tigera.io/calico/latest/getting-started/kubernetes/hardway/standing-up-kubernetes) in calico setup guide, run `sudo kubeadm init --pod-network-cidr=192.168.0.0/16` to initialize control-plane with calico network add-on.
* If you get an error saying multiple CRI endpoints on host, then run `sudo kubeadm init --pod-network-cidr=192.168.0.0/16 --cri-socket=unix:///var/run/cri-dockerd.sock`.
* After successfull execution of above command, there will be few instructions near end of the output. Follow them. There will also be a kubeadm join token, copy that and keep it somewhere safe for future use.
* Official guide of creating cluster with kubeadm is available [here](https://kubernetes.io/docs/setup/production-environment/tools/kubeadm/create-cluster-kubeadm/).
* Install conntrack with command `sudo pacman -S conntrack-tools`.
* Now you can use kubectl. Try `kubectl get pods`.
* If you get erros saying '0/1 nodes are availabe: 1 node(s) had untolerated taint', it means you have only one node that is control-plane node. In this case, you can either create new worker nodes or make control-plane node tainted with `kubectl taint nodes --all node-role.kubernetes.io/control-plane-`.
* Now we need to install Calico. Follow 'Install Calico' -> 'Manifest' of [this guide](https://docs.tigera.io/calico/latest/getting-started/kubernetes/self-managed-onprem/onpremises#install-calico) to install calico.
* Now do `kubectl get pods -A`, all pods should be ready (It may take few minutes so be patient before all pods become ready).
* For [Persistent Volume Claim](https://kubernetes.io/docs/concepts/storage/persistent-volumes/), we are using local storage. To configure this, move to 'K8S' folder and perform `kubectl apply -f storage-class.yaml`.
* In `local-pv.yaml` there is _path_ property, change it to the path that exist in your system. Then perform `chmod 777 <path>` to make it readable/writable/executable by all users. 
* Also in `local-pv.yaml` there is _value_ property nested inside _nodeAffinity_. This should match with one of your kubernetes nodes. Get nodes list with `kubectl get nodes`. Make sure name matches and perform `kubectl apply -f local-pv.yaml`. And finally `kubectl apply -f local-pvc.yaml`.

## Running Project
* Build docker image with `docker build -t <dockerhub user id>/<image name> .` to build project image.
* Push docker image to docker hub with `docker push <dockerhub user id>/<image name>`.
* Start deploying with kubernetes with `kubectl apply -f <yaml file name` excluding _storage-class_, _local-pv_ and _local-pvc_.
* Get all deployments with `kubectl get deployments`.
* Get all pods with `kubectl get pods`.
* View logs with `kubectl logs <pod name>`.
* View details of any kubernetes item with `kubectl describe <item type> <item name>`. Item type includes pod, service, pv(persistent volume), pvc(persistent volume claim), storageclass, deployment etc.
* `kubectl get services` will list all services. Listed services can be acccessed by port mentioned after ':'. For example, if service is listed as 80:32714/TCP, then that service is available at port 32714.
* If any change is done in dotnet service, build docker image, push to docker hub and do `kubectl rollout restart deployment <corresponding deployment name>`.