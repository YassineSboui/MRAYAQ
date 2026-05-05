<script setup lang="ts">
import logoLight from "../assets/logo-light.png";
import logoDark from "../assets/logo-dark.png";
</script>

<template>
  <header class="hero">
    <div class="container hero-inner">
      <div class="hero-card reveal">
        <div class="eyebrow">Styled. Tunis. Men.</div>
        <h1 class="hero-title">MRAYAQ</h1>
        <p class="hero-sub">
          Marque tunisienne pour hommes. Street-luxe, héritage et attitude Gen
          Z.
          <span class="arabic">مرايق — ستايل تونسي بروح جديدة.</span>
        </p>
        <div class="hero-actions">
          <RouterLink to="/catalog">
            <Button label="Shop Drop" severity="primary" />
          </RouterLink>
          <RouterLink to="/culture">
            <Button label="Our Culture" severity="secondary" outlined />
          </RouterLink>
        </div>
      </div>
      <div class="hero-logos reveal">
        <img :src="logoLight" alt="MRAYAQ logo light" />
        <img :src="logoDark" alt="MRAYAQ logo dark" />
      </div>
    </div>
  </header>
</template>
