<script setup lang="ts">
import CollectionsSection from "../components/CollectionsSection.vue";
import { products } from "../data/content";
</script>

<template>
  <div class="page">
    <CollectionsSection :items="products" />
  </div>
</template>
