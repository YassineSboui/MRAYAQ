<script setup lang="ts">
import { computed } from "vue";
import { useToast } from "primevue/usetoast";
import { RouterLink } from "vue-router";
import type { CartItem } from "../types.ts";

const props = defineProps<{ cart: CartItem[] }>();
const toast = useToast();

const count = computed(() =>
  props.cart.reduce((sum, item) => sum + item.quantity, 0),
);

const handleCheckout = () => {
  if (count.value === 0) {
    toast.add({
      severity: "warn",
      summary: "Panier vide",
      detail: "Ajoutez des articles avant de passer à la caisse",
      life: 2000,
    });
  }
};
</script>

<template>
  <div class="cart-bar">
    <span>{{ count }} items</span>
    <RouterLink to="/checkout" @click="handleCheckout">
      <Button label="Checkout" severity="primary" />
    </RouterLink>
  </div>
</template>
