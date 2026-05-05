<script setup lang="ts">
import type { Product } from "../types.ts";

defineProps<{ items: Product[] }>();
</script>

<template>
  <section id="collections" class="section">
    <div class="container">
      <div class="section-head">
        <div>
          <div class="eyebrow">Collections</div>
          <h2 class="section-title">Men Essentials, Tounsi Spirit</h2>
        </div>
        <div class="tag">Limited Drops</div>
      </div>
      <div class="grid">
        <article v-for="item in items" :key="item.name" class="card reveal">
          <div class="tag">{{ item.tag }}</div>
          <h3>{{ item.name }}</h3>
          <p>{{ item.desc }}</p>
          <strong>{{ item.price }}</strong>
        </article>
      </div>
    </div>
  </section>
</template>
