<script setup lang="ts">
import HeroSection from "../components/HeroSection.vue";
import CarouselSection from "../components/CarouselSection.vue";
import CollectionsSection from "../components/CollectionsSection.vue";
import CultureSection from "../components/CultureSection.vue";
import LookbookSection from "../components/LookbookSection.vue";
import QuotesSection from "../components/QuotesSection.vue";
import { cultureCards, products, quotes } from "../data/content";
</script>

<template>
  <div class="page">
    <CarouselSection />
    <HeroSection />
    <CollectionsSection :items="products" />
    <CultureSection :items="cultureCards" />
    <LookbookSection />
    <QuotesSection :items="quotes" />
  </div>
</template>
