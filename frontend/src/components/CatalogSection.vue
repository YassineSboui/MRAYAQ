<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { useToast } from "primevue/usetoast";
import type { CategoryDto, ProductDto } from "../types.ts";
import { api } from "../services/api";

const { onAddToCart } = defineProps<{
  onAddToCart: (product: ProductDto) => void;
}>();

const toast = useToast();
const categories = ref<CategoryDto[]>([]);
const products = ref<ProductDto[]>([]);
const loading = ref(false);
const error = ref("");

const filters = ref({
  search: "",
  categoryId: null as string | null,
  minPrice: null as number | null,
  maxPrice: null as number | null,
});

const filteredCount = computed(() => products.value.length);

const load = async () => {
  loading.value = true;
  error.value = "";

  try {
    const [cats, items] = await Promise.all([
      api.getCategories(),
      api.getProducts({
        search: filters.value.search,
        categoryId: filters.value.categoryId ?? undefined,
        minPrice: filters.value.minPrice ?? undefined,
        maxPrice: filters.value.maxPrice ?? undefined,
      }),
    ]);

    categories.value = cats;
    products.value = items;

    if (items.length === 0) {
      toast.add({
        severity: "info",
        summary: "Aucun résultat",
        detail: "Aucun produit ne correspond à vos critères",
        life: 2000,
      });
    }
  } catch (err) {
    console.error(err);
    error.value = "Impossible de charger le catalogue. Vérifiez le serveur.";
    toast.add({
      severity: "error",
      summary: "Erreur",
      detail: error.value,
      life: 3000,
    });
  } finally {
    loading.value = false;
  }
};

const resetFilters = () => {
  filters.value = {
    search: "",
    categoryId: null,
    minPrice: null,
    maxPrice: null,
  };
  void load();
  toast.add({
    severity: "info",
    summary: "Filtres réinitialisés",
    detail: "Affichage de tous les produits",
    life: 2000,
  });
};

const handleAddToCart = (product: ProductDto) => {
  onAddToCart(product);
  toast.add({
    severity: "success",
    summary: "Succès",
    detail: `${product.name} ajouté au panier`,
    life: 2000,
  });
};

onMounted(() => {
  void load();
});
</script>

<template>
  <section id="catalog" class="section">
    <Toast />
    <div class="container">
      <div class="section-head">
        <div>
          <div class="eyebrow">Catalog</div>
          <h2 class="section-title">Shop MRAYAQ Drops</h2>
        </div>
        <div class="tag">{{ filteredCount }} styles</div>
      </div>

      <div class="catalog-layout">
        <aside class="filters card">
          <h3>Filtres</h3>
          <InputText
            v-model="filters.search"
            placeholder="Recherche..."
            @input="load"
          />
          <label class="filter-label" for="filter-category">Categorie</label>
          <Select
            id="filter-category"
            v-model="filters.categoryId"
            :options="categories"
            optionLabel="name"
            optionValue="id"
            placeholder="Toutes les categories"
            showClear
            @change="load"
          />
          <label class="filter-label" for="filter-min">Prix</label>
          <InputNumber
            id="filter-min"
            v-model="filters.minPrice"
            :min="0"
            placeholder="Prix min"
            @update:modelValue="load"
          />
          <InputNumber
            id="filter-max"
            v-model="filters.maxPrice"
            :min="0"
            placeholder="Prix max"
            @update:modelValue="load"
          />
          <Button
            label="Reset"
            severity="secondary"
            text
            @click="resetFilters"
          />
        </aside>

        <div>
          <p v-if="error" class="status error">{{ error }}</p>
          <p v-else-if="loading" class="status">Chargement...</p>

          <div v-else class="catalog-grid">
            <article
              v-for="item in products"
              :key="item.id"
              class="product-card reveal"
            >
              <img :src="item.imageUrl" :alt="item.name" />
              <div class="product-info">
                <Tag :value="item.tag" severity="warning" />
                <h3>{{ item.name }}</h3>
                <p>{{ item.description }}</p>
                <div class="product-meta">
                  <strong>{{ item.price }} DT</strong>
                  <span>{{ item.categoryName }}</span>
                </div>
                <Button
                  label="Ajouter"
                  severity="primary"
                  @click="handleAddToCart(item)"
                />
              </div>
            </article>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
