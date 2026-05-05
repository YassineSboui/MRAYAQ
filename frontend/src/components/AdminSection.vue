<script setup lang="ts">
import { onMounted, ref, computed } from "vue";
import type {
  CategoryDto,
  CreateCategoryRequest,
  CreateProductRequest,
  LoginRequest,
  OrderDto,
  ProductDto,
} from "../types.ts";
import { api } from "../services/api";
import { auth } from "../services/auth";
import { useToast } from "primevue/usetoast";

const emit = defineEmits<{
  (e: "login"): void;
  (e: "logout"): void;
}>();

const toast = useToast();
const isAdmin = ref(auth.isLoggedIn());
const activeTab = ref("0");
const categories = ref<CategoryDto[]>([]);
const products = ref<ProductDto[]>([]);
const orders = ref<OrderDto[]>([]);

// Orders Tab
const showOrderDetail = ref(false);
const selectedOrder = ref<OrderDto | null>(null);
const statusFilter = ref("all");
const ordersFiltered = computed(() => {
  if (statusFilter.value === "all") return orders.value;
  return orders.value.filter((o) => o.status === statusFilter.value);
});

// Login
const loginForm = ref<LoginRequest>({ username: "", password: "" });
const loginStatus = ref("");

// Product Modal
const showProductModal = ref(false);
const editingProductId = ref<string | null>(null);
const productForm = ref<CreateProductRequest>({
  name: "",
  description: "",
  price: 0,
  tag: "",
  imageUrl: "",
  isFeatured: false,
  categoryId: "",
});

// Category Modal
const showCategoryModal = ref(false);
const editingCategoryId = ref<string | null>(null);
const categoryForm = ref<CreateCategoryRequest>({ name: "", slug: "" });

// Delete Confirmation
const showDeleteConfirm = ref(false);
const deleteItem = ref<{ type: "product" | "category"; id: string } | null>(
  null,
);

const load = async () => {
  try {
    const [cats, items, ordersData] = await Promise.all([
      api.getCategories(),
      api.getProducts({}),
      api.getOrders(),
    ]);
    categories.value = cats;
    products.value = items;
    orders.value = ordersData;
  } catch (error) {
    console.error(error);
    toast.add({
      severity: "error",
      summary: "Erreur",
      detail: "Erreur lors du chargement des données",
      life: 3000,
    });
  }
};

const login = async () => {
  loginStatus.value = "";
  try {
    const result = await api.login(loginForm.value);
    if (!result?.token) {
      loginStatus.value = "Identifiants invalides.";
      return;
    }
    auth.setToken(result.token);
    isAdmin.value = true;
    emit("login");
    await load();
  } catch (error) {
    console.error(error);
    loginStatus.value = "Connexion admin échouée.";
  }
};

const logout = () => {
  auth.clearToken();
  isAdmin.value = false;
  emit("logout");
};

// Product Management
const openProductModal = (product?: ProductDto) => {
  if (product) {
    editingProductId.value = product.id;
    productForm.value = {
      name: product.name,
      description: product.description,
      price: product.price,
      tag: product.tag,
      imageUrl: product.imageUrl,
      isFeatured: product.isFeatured,
      categoryId: product.categoryId,
    };
  } else {
    editingProductId.value = null;
    productForm.value = {
      name: "",
      description: "",
      price: 0,
      tag: "",
      imageUrl: "",
      isFeatured: false,
      categoryId: categories.value[0]?.id ?? "",
    };
  }
  showProductModal.value = true;
};

const saveProduct = async () => {
  if (!productForm.value.name || !productForm.value.categoryId) {
    toast.add({
      severity: "warn",
      summary: "Attention",
      detail: "Nom et catégorie sont requis",
      life: 3000,
    });
    return;
  }

  try {
    if (editingProductId.value) {
      await api.updateProduct(editingProductId.value, productForm.value);
      toast.add({
        severity: "success",
        summary: "Succès",
        detail: "Produit mis à jour",
        life: 2000,
      });
    } else {
      await api.createProduct(productForm.value);
      toast.add({
        severity: "success",
        summary: "Succès",
        detail: "Produit créé",
        life: 2000,
      });
    }
    showProductModal.value = false;
    await load();
  } catch (error) {
    console.error(error);
    toast.add({
      severity: "error",
      summary: "Erreur",
      detail: "Erreur lors de la sauvegarde",
      life: 3000,
    });
  }
};

const deleteProduct = async (id: string) => {
  deleteItem.value = { type: "product", id };
  showDeleteConfirm.value = true;
};

const confirmDelete = async () => {
  if (!deleteItem.value) return;

  try {
    if (deleteItem.value.type === "product") {
      await api.deleteProduct(deleteItem.value.id);
      toast.add({
        severity: "success",
        summary: "Succès",
        detail: "Produit supprimé",
        life: 2000,
      });
    } else {
      await api.deleteCategory(deleteItem.value.id);
      toast.add({
        severity: "success",
        summary: "Succès",
        detail: "Catégorie supprimée",
        life: 2000,
      });
    }
    showDeleteConfirm.value = false;
    deleteItem.value = null;
    await load();
  } catch (error) {
    console.error(error);
    toast.add({
      severity: "error",
      summary: "Erreur",
      detail: "Erreur lors de la suppression",
      life: 3000,
    });
  }
};

// Category Management
const openCategoryModal = (category?: CategoryDto) => {
  if (category) {
    editingCategoryId.value = category.id;
    categoryForm.value = { name: category.name, slug: category.slug };
  } else {
    editingCategoryId.value = null;
    categoryForm.value = { name: "", slug: "" };
  }
  showCategoryModal.value = true;
};

const saveCategory = async () => {
  if (!categoryForm.value.name || !categoryForm.value.slug) {
    toast.add({
      severity: "warn",
      summary: "Attention",
      detail: "Nom et slug sont requis",
      life: 3000,
    });
    return;
  }

  try {
    if (editingCategoryId.value) {
      await api.updateCategory(editingCategoryId.value, categoryForm.value);
      toast.add({
        severity: "success",
        summary: "Succès",
        detail: "Catégorie mise à jour",
        life: 2000,
      });
    } else {
      await api.createCategory(categoryForm.value);
      toast.add({
        severity: "success",
        summary: "Succès",
        detail: "Catégorie créée",
        life: 2000,
      });
    }
    showCategoryModal.value = false;
    await load();
  } catch (error) {
    console.error(error);
    toast.add({
      severity: "error",
      summary: "Erreur",
      detail: "Erreur lors de la sauvegarde",
      life: 3000,
    });
  }
};

const deleteCategory = async (id: string) => {
  deleteItem.value = { type: "category", id };
  showDeleteConfirm.value = true;
};

// Order Management
const selectOrder = (order: OrderDto) => {
  selectedOrder.value = order;
  showOrderDetail.value = true;
};

const getOrderStepValue = (status: string): number => {
  const statusMap: { [key: string]: number } = {
    pending: 0,
    verified: 1,
    delivered: 2,
  };
  return statusMap[status] || 0;
};

const updateOrderStatus = async (status: string) => {
  if (!selectedOrder.value) return;

  try {
    const updated = await api.updateOrderStatus(selectedOrder.value.id, status);
    selectedOrder.value = updated;
    // Update in list
    const idx = orders.value.findIndex((o) => o.id === updated.id);
    if (idx !== -1) {
      orders.value[idx] = updated;
    }
    toast.add({
      severity: "success",
      summary: "Succès",
      detail: "Statut de commande mis à jour",
      life: 2000,
    });
  } catch (error) {
    console.error(error);
    toast.add({
      severity: "error",
      summary: "Erreur",
      detail: "Erreur lors de la mise à jour",
      life: 3000,
    });
  }
};

const getStatusIcon = (status: string): string => {
  const icons: { [key: string]: string } = {
    pending: "pi-hourglass",
    verified: "pi-check",
    delivered: "pi-box",
  };
  return icons[status] || "pi-question";
};

const getStatusSeverity = (status: string): string => {
  const severities: { [key: string]: string } = {
    pending: "warning",
    verified: "info",
    delivered: "success",
  };
  return severities[status] || "secondary";
};

onMounted(() => {
  if (isAdmin.value) {
    void load();
  }
});
</script>

<template>
  <section id="admin" class="section section-alt">
    <Toast />
    <div class="container">
      <div class="section-head">
        <div>
          <div class="eyebrow">Admin</div>
          <h2 class="section-title">Gestion MRAYAQ</h2>
        </div>
        <Button
          v-if="isAdmin"
          label="Déconnexion"
          severity="secondary"
          @click="logout"
        />
      </div>

      <div v-if="!isAdmin" class="admin-login">
        <div class="card" style="max-width: 400px; margin: 0 auto">
          <h3>Admin Connexion</h3>
          <form @submit.prevent="login" style="display: grid; gap: 12px">
            <InputText
              v-model="loginForm.username"
              placeholder="Nom d'utilisateur"
            />
            <Password
              v-model="loginForm.password"
              placeholder="Mot de passe"
              toggleMask
            />
            <Button
              label="Connexion"
              severity="primary"
              type="submit"
              style="width: 100%"
            />
            <Message v-if="loginStatus" severity="error">
              {{ loginStatus }}
            </Message>
          </form>
        </div>
      </div>

      <Tabs v-else v-model:value="activeTab">
        <TabList>
          <Tab value="0"><span class="pi pi-box tab-icon"></span>Produits</Tab>
          <Tab value="1"><span class="pi pi-inbox tab-icon"></span>Catégories</Tab>
          <Tab value="2"><span class="pi pi-list tab-icon"></span>Commandes</Tab>
        </TabList>

        <TabPanels>
        <!-- Products Tab -->
        <TabPanel value="0">
          <br />
          <Button
            label="Ajouter un produit"
            icon="pi pi-plus"
            severity="primary"
            class="mb-4"
            @click="openProductModal()"
          />

          <div
            style="
              display: grid;
              grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
              gap: 20px;
            "
          >
            <div v-for="product in products" :key="product.id" class="card">
              <img
                v-if="product.imageUrl"
                :src="product.imageUrl"
                alt="product"
                style="
                  width: 100%;
                  height: 200px;
                  object-fit: cover;
                  border-radius: var(--radius-md);
                  margin-bottom: 12px;
                "
              />
              <h4 style="margin-top: 0">{{ product.name }}</h4>
              <p style="color: var(--muted); font-size: 0.9rem">
                {{ product.description }}
              </p>
              <div
                style="
                  display: flex;
                  justify-content: space-between;
                  align-items: center;
                  margin: 12px 0;
                "
              >
                <span style="color: var(--gold); font-weight: bold"
                  >{{ product.price }} DT</span
                >
                <span class="p-tag">{{ product.categoryName }}</span>
              </div>
              <div style="display: flex; gap: 8px">
                <Button
                  label="Modifier"
                  size="small"
                  severity="secondary"
                  @click="openProductModal(product)"
                />
                <Button
                  label="Supprimer"
                  size="small"
                  severity="danger"
                  @click="deleteProduct(product.id)"
                />
              </div>
            </div>
          </div>
        </TabPanel>

        <!-- Categories Tab -->
        <TabPanel value="1">
          <br />
          <Button
            label="Ajouter une catégorie"
            icon="pi pi-plus"
            severity="primary"
            class="mb-4"
            @click="openCategoryModal()"
          />

          <div
            style="
              display: grid;
              grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
              gap: 16px;
            "
          >
            <div v-for="cat in categories" :key="cat.id" class="card">
              <h4 style="margin-top: 0">{{ cat.name }}</h4>
              <p style="color: var(--muted); font-size: 0.85rem">
                {{ cat.slug }}
              </p>
              <div style="display: flex; gap: 8px; margin-top: 12px">
                <Button
                  label="Modifier"
                  size="small"
                  severity="secondary"
                  @click="openCategoryModal(cat)"
                />
                <Button
                  label="Supprimer"
                  size="small"
                  severity="danger"
                  @click="deleteCategory(cat.id)"
                />
              </div>
            </div>
          </div>
        </TabPanel>

        <!-- Orders Tab -->
        <TabPanel value="2">
          <br />
          <div
            style="
              display: flex;
              gap: 12px;
              margin-bottom: 20px;
              align-items: center;
            "
          >
            <label for="statusFilter" style="color: var(--muted); font-size: 0.9rem"
              >Filtrer par statut:</label
            >
            <Select
              id="statusFilter"
              v-model="statusFilter"
              :options="[
                { label: 'Tous', value: 'all' },
                { label: 'En attente', value: 'pending' },
                { label: 'Vérifié', value: 'verified' },
                { label: 'Livré', value: 'delivered' },
              ]"
              optionLabel="label"
              optionValue="value"
              style="min-width: 200px"
            />
          </div>

          <div
            v-if="ordersFiltered.length === 0"
            style="text-align: center; color: var(--muted); padding: 40px"
          >
            <p>Aucune commande trouvée</p>
          </div>

          <DataTable
            v-else
            :value="ordersFiltered"
            striped-rows
            responsive-layout="scroll"
            paginator
            :rows="10"
            paginator-template="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
            style="font-size: 0.85rem"
          >
            <Column field="id" header="N° Commande" style="width: 15%">
              <template #body="{ data }">
                <span style="font-family: monospace; font-size: 0.75rem">{{
                  data.id.substring(0, 8)
                }}</span>
              </template>
            </Column>
            <Column field="customerName" header="Client" style="width: 15%" />
            <Column field="email" header="Email" style="width: 20%" />
            <Column field="totalAmount" header="Montant" style="width: 10%">
              <template #body="{ data }">
                <span style="color: var(--gold); font-weight: bold"
                  >{{ data.totalAmount }} DT</span
                >
              </template>
            </Column>
            <Column field="status" header="Statut" style="width: 15%">
              <template #body="{ data }">
                <Tag
                  :value="data.status"
                  :severity="getStatusSeverity(data.status)"
                  :icon="`pi ${getStatusIcon(data.status)}`"
                />
              </template>
            </Column>
            <Column style="width: 10%">
              <template #body="{ data }">
                <Button
                  label="Détails"
                  size="small"
                  severity="secondary"
                  @click="selectOrder(data)"
                />
              </template>
            </Column>
          </DataTable>
        </TabPanel>
        </TabPanels>
      </Tabs>

      <!-- Product Modal -->
      <Dialog
        v-model:visible="showProductModal"
        modal
        :header="editingProductId ? 'Modifier produit' : 'Nouveau produit'"
        :style="{ width: '90vw', maxWidth: '600px' }"
      >
        <div style="display: grid; gap: 16px">
          <div>
            <label for="productName" class="form-label">Nom</label>
            <InputText
              id="productName"
              v-model="productForm.name"
              placeholder="Nom du produit"
            />
          </div>

          <div>
            <label for="productDescription" class="form-label">Description</label>
            <Textarea
              id="productDescription"
              v-model="productForm.description"
              placeholder="Description"
              rows="4"
            />
          </div>

          <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 12px">
            <div>
              <label for="productPrice" class="form-label">Prix (DT)</label>
              <InputNumber
                id="productPrice"
                v-model="productForm.price"
                :min="0"
                placeholder="Prix"
              />
            </div>
            <div>
              <label for="productCategory" class="form-label">Catégorie</label>
              <Select
                id="productCategory"
                v-model="productForm.categoryId"
                :options="categories"
                optionLabel="name"
                optionValue="id"
                placeholder="Sélectionner"
              />
            </div>
          </div>

          <div>
            <label for="productTag" class="form-label">Tag</label>
            <InputText
              id="productTag"
              v-model="productForm.tag"
              placeholder="Ex: Limited Edition"
            />
          </div>

          <div>
            <label for="productImageUrl" class="form-label">URL Image</label>
            <InputText
              id="productImageUrl"
              v-model="productForm.imageUrl"
              placeholder="https://..."
            />
            <img
              v-if="productForm.imageUrl"
              :src="productForm.imageUrl"
              alt="preview"
              style="
                margin-top: 12px;
                max-width: 100%;
                max-height: 200px;
                border-radius: var(--radius-md);
                object-fit: cover;
              "
            />
          </div>

          <label
            for="productFeatured"
            style="
              display: flex;
              gap: 8px;
              align-items: center;
              cursor: pointer;
            "
          >
            <Checkbox id="productFeatured" v-model="productForm.isFeatured" binary />
            <span style="color: var(--text)">Produit en vedette</span>
          </label>
        </div>

        <template #footer>
          <Button
            label="Annuler"
            severity="secondary"
            @click="showProductModal = false"
          />
          <Button
            :label="editingProductId ? 'Mettre à jour' : 'Créer'"
            severity="primary"
            @click="saveProduct"
          />
        </template>
      </Dialog>

      <!-- Category Modal -->
      <Dialog
        v-model:visible="showCategoryModal"
        modal
        :header="
          editingCategoryId ? 'Modifier catégorie' : 'Nouvelle catégorie'
        "
        :style="{ width: '90vw', maxWidth: '500px' }"
      >
        <div style="display: grid; gap: 16px">
          <div>
            <label for="categoryName" class="form-label">Nom</label>
            <InputText
              id="categoryName"
              v-model="categoryForm.name"
              placeholder="Nom de la catégorie"
            />
          </div>

          <div>
            <label for="categorySlug" class="form-label">Slug</label>
            <InputText
              id="categorySlug"
              v-model="categoryForm.slug"
              placeholder="slug-url-friendly"
            />
          </div>
        </div>

        <template #footer>
          <Button
            label="Annuler"
            severity="secondary"
            @click="showCategoryModal = false"
          />
          <Button
            :label="editingCategoryId ? 'Mettre à jour' : 'Créer'"
            severity="primary"
            @click="saveCategory"
          />
        </template>
      </Dialog>

      <!-- Delete Confirmation -->
      <Dialog
        v-model:visible="showDeleteConfirm"
        modal
        header="Confirmer la suppression"
        :style="{ width: '90vw', maxWidth: '400px' }"
      >
        <p>
          Êtes-vous sûr de vouloir supprimer cet élément? Cette action est
          irréversible.
        </p>

        <template #footer>
          <Button
            label="Annuler"
            severity="secondary"
            @click="showDeleteConfirm = false"
          />
          <Button label="Supprimer" severity="danger" @click="confirmDelete" />
        </template>
      </Dialog>

      <!-- Order Detail Dialog -->
      <Dialog
        v-model:visible="showOrderDetail"
        modal
        header="Détails de la commande"
        :style="{ width: '90vw', maxWidth: '700px' }"
      >
        <div v-if="selectedOrder" style="display: grid; gap: 20px">
          <!-- Order Info -->
          <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 16px">
            <div>
              <div class="form-label">Numéro de commande</div>
              <span style="font-family: monospace; color: var(--text)">{{
                selectedOrder.id.substring(0, 8)
              }}</span>
            </div>
            <div>
              <div class="form-label">Date</div>
              <span>{{
                new Date(selectedOrder.createdAt).toLocaleDateString()
              }}</span>
            </div>
            <div>
              <div class="form-label">Client</div>
              <span>{{ selectedOrder.customerName }}</span>
            </div>
            <div>
              <div class="form-label">Email</div>
              <span>{{ selectedOrder.email }}</span>
            </div>
            <div>
              <div class="form-label">Téléphone</div>
              <span>{{ selectedOrder.phone }}</span>
            </div>
            <div>
              <div class="form-label">Montant</div>
              <span
                style="color: var(--gold); font-weight: bold; font-size: 1.1rem"
                >{{ selectedOrder.totalAmount }} DT</span
              >
            </div>
          </div>

          <!-- Address -->
          <div>
            <div class="form-label">Adresse de livraison</div>
            <div style="color: var(--text)">
              <p style="margin: 0">{{ selectedOrder.address }}</p>
              <p style="margin: 4px 0 0 0">{{ selectedOrder.city }}</p>
            </div>
          </div>

          <!-- Items -->
          <div v-if="selectedOrder.items.length > 0">
            <div class="form-label">Articles</div>
            <div style="display: grid; gap: 8px">
              <div
                v-for="item in selectedOrder.items"
                :key="item.productId"
                style="
                  display: flex;
                  justify-content: space-between;
                  align-items: center;
                  padding: 8px;
                  background: rgba(255, 255, 255, 0.04);
                  border-radius: var(--radius-sm);
                "
              >
                <div>
                  <p style="margin: 0; font-weight: 500">
                    {{ item.productName }}
                  </p>
                  <p
                    style="
                      margin: 4px 0 0 0;
                      color: var(--muted);
                      font-size: 0.85rem;
                    "
                  >
                    Quantité: {{ item.quantity }}
                  </p>
                </div>
                <span style="color: var(--gold)"
                  >{{ item.unitPrice * item.quantity }} DT</span
                >
              </div>
            </div>
          </div>

          <!-- Status Workflow with Stepper -->
          <div>
            <div class="form-label">Workflow de traitement</div>
            <div
              style="
                margin-top: 20px;
                padding: 32px 24px;
                background: linear-gradient(135deg, rgba(47, 123, 94, 0.08) 0%, rgba(201, 155, 71, 0.04) 100%);
                border: 1px solid rgba(255, 255, 255, 0.12);
                border-radius: var(--radius-lg);
                display: flex;
                flex-direction: column;
                gap: 28px;
              "
            >
              <Stepper :value="getOrderStepValue(selectedOrder.status)" linear>
                <StepList style="justify-content: space-around">
                  <Step :value="0">En attente</Step>
                  <Step :value="1">Vérifié</Step>
                  <Step :value="2">Livré</Step>
                </StepList>

                <StepPanels>
                  <StepPanel :value="0">
                    <div
                      style="
                        display: flex;
                        flex-direction: column;
                        gap: 16px;
                        padding: 24px 16px;
                        text-align: center;
                        align-items: center;
                      "
                    >
                      <p style="color: var(--muted); margin: 0; font-size: 0.95rem;">
                        Commande en attente de vérification
                      </p>
                      <div style="display: flex; gap: 12px; justify-content: center; flex-wrap: wrap;">
                        <Button
                          label="Vérifier"
                          @click="updateOrderStatus('verified')"
                          severity="primary"
                          style="min-width: 120px"
                        />
                      </div>
                    </div>
                  </StepPanel>

                  <StepPanel :value="1">
                    <div
                      style="
                        display: flex;
                        flex-direction: column;
                        gap: 16px;
                        padding: 24px 16px;
                        text-align: center;
                        align-items: center;
                      "
                    >
                      <p style="color: var(--muted); margin: 0; font-size: 0.95rem;">
                        Commande vérifiée, prêt pour livraison
                      </p>
                      <div style="display: flex; gap: 12px; justify-content: center; flex-wrap: wrap;">
                        <Button
                          label="Marquer comme livré"
                          @click="updateOrderStatus('delivered')"
                          severity="success"
                          style="min-width: 140px"
                        />
                        <Button
                          label="Retour à en attente"
                          @click="updateOrderStatus('pending')"
                          severity="secondary"
                          style="min-width: 140px"
                        />
                      </div>
                    </div>
                  </StepPanel>

                  <StepPanel :value="2">
                    <div
                      style="
                        display: flex;
                        flex-direction: column;
                        gap: 16px;
                        padding: 24px 16px;
                        text-align: center;
                        align-items: center;
                      "
                    >
                      <p style="color: var(--muted); margin: 0; font-size: 0.95rem;">
                        Commande livrée au client
                      </p>
                      <div style="display: flex; gap: 12px; justify-content: center; flex-wrap: wrap;">
                        <Button
                          label="Retour à vérification"
                          @click="updateOrderStatus('verified')"
                          severity="secondary"
                          style="min-width: 140px"
                        />
                      </div>
                    </div>
                  </StepPanel>
                </StepPanels>
              </Stepper>
            </div>
          </div>

          <!-- Notes -->
          <div v-if="selectedOrder.notes">
            <div class="form-label">Notes</div>
            <p style="color: var(--text); margin: 0">
              {{ selectedOrder.notes }}
            </p>
          </div>
        </div>

        <template #footer>
          <Button
            label="Fermer"
            severity="secondary"
            @click="showOrderDetail = false"
          />
        </template>
      </Dialog>
    </div>
  </section>
</template>

<style scoped>
.admin-login {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 400px;
}

.card {
  background: rgba(255, 255, 255, 0.04);
  border-radius: var(--radius-md);
  padding: 20px;
  border: 1px solid rgba(255, 255, 255, 0.08);
  box-shadow: var(--shadow);
}

.w-full {
  width: 100%;
}

.mb-4 {
  margin-bottom: 16px;
}

.form-label {
  display: block;
  margin-bottom: 8px;
  color: var(--muted);
  font-size: 0.85rem;
  text-transform: uppercase;
  letter-spacing: 0.08em;
}

:deep(.p-tabs) {
  background: transparent;
}

:deep(.p-tabs .p-tablist) {
  background: rgba(255, 255, 255, 0.04);
  border-bottom: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: var(--radius-md);
  margin-bottom: 20px;
}

:deep(.p-tabs .p-tabpanels) {
  background: transparent;
  padding: 0;
}

:deep(.p-tabs .p-tabpanel) {
  padding: 20px 0;
}

.tab-icon {
  margin-right: 6px;
}
</style>
