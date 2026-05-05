<script setup lang="ts">
import CatalogSection from "../components/CatalogSection.vue";
import CartBar from "../components/CartBar.vue";
import { useCart } from "../stores/cart";

const cart = useCart();
</script>

<template>
  <div class="page">
    <CatalogSection :on-add-to-cart="cart.add" />
    <CartBar :cart="cart.items" />
  </div>
</template>
