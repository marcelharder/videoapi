Kubernetes deployment notes

1) Build and push the image (replace registry and tag):

```bash
docker build -t registry.example.com/videoapi:latest .
docker push registry.example.com/videoapi:latest
```

2) Apply the NFS PV and PVC (adjust `server` and `path` in `k8s/pv-pvc.yaml`):

```bash
kubectl apply -f k8s/pv-pvc.yaml
```

3) Create a namespace (optional) and apply the secret, service, and deployment:

```bash
kubectl create ns videoapi || true
kubectl -n videoapi apply -f k8s/secret.yaml
kubectl -n videoapi apply -f k8s/service.yaml
kubectl -n videoapi apply -f k8s/deployment.yaml
```

Notes:
- The deployment reads the connection string from the Kubernetes `Secret` (`ConnectionStrings__DefaultConnection`).
- The `VideoSettings__NasBasePath` env var is set to `/mnt/nas/videos` and the PVC is mounted at the same path inside the container.
- If using a cloud provider, consider a cloud native NFS or dynamic provisioner rather than binding to a static PV.
- For production, use image pull secrets, RBAC, resource requests/limits, and health/readiness probes.
