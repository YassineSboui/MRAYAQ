<script setup lang="ts">
import { ref } from "vue";
import { RouterView } from "vue-router";
import AppNav from "./components/AppNav.vue";
import AppFooter from "./components/AppFooter.vue";
import { auth } from "./services/auth";

const isAdmin = ref(auth.isLoggedIn());

const handleLogin = () => {
  isAdmin.value = true;
};

const handleLogout = () => {
  isAdmin.value = false;
};
</script>

<template>
  <div class="app-shell">
    <Toast position="top-right" />
    <AppNav :is-admin="isAdmin" />
    <main class="page">
      <RouterView v-slot="{ Component }">
        <component
          :is="Component"
          @login="handleLogin"
          @logout="handleLogout"
        />
      </RouterView>
    </main>
    <AppFooter />
  </div>
</template>
