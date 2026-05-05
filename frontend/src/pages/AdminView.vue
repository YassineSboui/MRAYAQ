<script setup lang="ts">
import AdminSection from "../components/AdminSection.vue";

const emit = defineEmits<{
  (e: "login"): void;
  (e: "logout"): void;
}>();
</script>

<template>
  <div class="page">
    <AdminSection @login="emit('login')" @logout="emit('logout')" />
  </div>
</template>
