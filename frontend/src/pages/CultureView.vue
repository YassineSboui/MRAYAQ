<script setup lang="ts">
import CultureSection from "../components/CultureSection.vue";
import LookbookSection from "../components/LookbookSection.vue";
import QuotesSection from "../components/QuotesSection.vue";
import { cultureCards, quotes } from "../data/content";
</script>

<template>
  <div class="page">
    <CultureSection :items="cultureCards" />
    <LookbookSection />
    <QuotesSection :items="quotes" />
  </div>
</template>
