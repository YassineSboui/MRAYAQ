<script setup lang="ts">
import { ref } from "vue";
import { RouterLink } from "vue-router";
import logoDark from "../assets/logo-dark.png";

defineProps<{ isAdmin: boolean }>();

const isOpen = ref(false);

const toggleMenu = () => {
  isOpen.value = !isOpen.value;
};
</script>

<template>
  <nav class="nav">
    <div class="container nav-inner">
      <RouterLink class="brand" to="/">
        <img :src="logoDark" alt="MRAYAQ logo" />
        <span>MRAYAQ</span>
      </RouterLink>
      <div class="nav-links" :class="{ open: isOpen }">
        <RouterLink to="/" @click="isOpen = false">Home</RouterLink>
        <RouterLink to="/collections" @click="isOpen = false"
          >Collections</RouterLink
        >
        <RouterLink to="/catalog" @click="isOpen = false">Catalog</RouterLink>
        <RouterLink to="/culture" @click="isOpen = false">Culture</RouterLink>
        <RouterLink to="/lookbook" @click="isOpen = false">Lookbook</RouterLink>
        <RouterLink to="/checkout" @click="isOpen = false">Checkout</RouterLink>
        <RouterLink to="/contact" @click="isOpen = false">Contact</RouterLink>
        <RouterLink v-if="isAdmin" to="/admin" @click="isOpen = false"
          >Admin</RouterLink
        >
      </div>
      <div class="lang-pill">TN / FR</div>
      <Button
        class="nav-toggle"
        icon="pi pi-bars"
        severity="secondary"
        text
        @click="toggleMenu"
      />
    </div>
  </nav>
</template>
