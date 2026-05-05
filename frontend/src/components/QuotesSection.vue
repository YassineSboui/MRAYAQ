<script setup lang="ts">
import type { Quote } from "../types.ts";

defineProps<{ items: Quote[] }>();
</script>

<template>
  <section class="section section-alt">
    <div class="container">
      <div class="section-head">
        <div>
          <div class="eyebrow">Quotes</div>
          <h2 class="section-title">Tounsi Words, MRAYAQ Mood</h2>
        </div>
      </div>
      <div class="grid">
        <article
          v-for="quote in items"
          :key="quote.text"
          class="quote-card reveal"
        >
          <p class="arabic">{{ quote.text }}</p>
          <span>{{ quote.author }}</span>
        </article>
      </div>
    </div>
  </section>
</template>
