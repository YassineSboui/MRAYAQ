<script setup lang="ts">
import type { CultureCard } from "../types.ts";

defineProps<{ items: CultureCard[] }>();
</script>

<template>
  <section id="culture" class="section section-alt">
    <div class="container">
      <div class="section-head">
        <div>
          <div class="eyebrow">Culture</div>
          <h2 class="section-title">Tunisian Energy, Global Edge</h2>
        </div>
        <p class="hero-sub">
          Du souk à la street, MRAYAQ mélange textures locales et silhouettes
          modernes.
        </p>
      </div>
      <div class="grid">
        <article v-for="card in items" :key="card.title" class="card reveal">
          <h3 class="arabic">{{ card.title }}</h3>
          <p>{{ card.text }}</p>
        </article>
      </div>
    </div>
  </section>
</template>
