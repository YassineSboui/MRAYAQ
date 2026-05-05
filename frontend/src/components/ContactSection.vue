<script setup lang="ts">
import { ref } from "vue";
import { useToast } from "primevue/usetoast";
import type { ContactForm } from "../types.ts";

const toast = useToast();
const form = ref<ContactForm>({ name: "", email: "", phone: "", message: "" });
const sending = ref(false);

const submitContact = async () => {
  sending.value = true;

  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_BASE ?? "https://localhost:5001"}/api/contact`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(form.value),
      },
    );

    const payload = await response.json();

    if (response.ok) {
      toast.add({
        severity: "success",
        summary: "Message envoyé",
        detail: payload?.message ?? "Merci pour votre message!",
        life: 3000,
      });
      form.value = { name: "", email: "", phone: "", message: "" };
    } else {
      toast.add({
        severity: "error",
        summary: "Erreur",
        detail: payload?.message ?? "Une erreur est survenue.",
        life: 3000,
      });
    }
  } catch (error) {
    console.error(error);
    toast.add({
      severity: "error",
      summary: "Connexion échouée",
      detail: "Vérifiez le serveur et réessayez.",
      life: 3000,
    });
  } finally {
    sending.value = false;
  }
};
</script>

<template>
  <section id="contact" class="section">
    <Toast />
    <div class="container contact">
      <div>
        <div class="eyebrow">Contact</div>
        <h2 class="section-title">Let’s Build Your Fit</h2>
        <p class="hero-sub">
          Envoyez-nous un message pour les commandes, collaborations ou drops
          privés.
        </p>
        <p class="arabic">"ابعثنا و خلّي الستايل يحكي"</p>
      </div>
      <form @submit.prevent="submitContact" class="card">
        <InputText v-model="form.name" placeholder="Nom" required />
        <InputText
          v-model="form.email"
          type="email"
          placeholder="Email"
          required
        />
        <InputText v-model="form.phone" placeholder="Téléphone (optionnel)" />
        <Textarea v-model="form.message" placeholder="Votre message" required />
        <Button
          label="Envoyer"
          severity="primary"
          type="submit"
          :loading="sending"
        />
      </form>
    </div>
  </section>
</template>
