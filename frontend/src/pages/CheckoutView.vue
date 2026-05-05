<script setup lang="ts">
import CheckoutSection from "../components/CheckoutSection.vue";
import { useCart } from "../stores/cart";

const cart = useCart();
</script>

<template>
  <div class="page">
    <CheckoutSection :cart="cart.items" :on-clear-cart="cart.clear" />
  </div>
</template>
