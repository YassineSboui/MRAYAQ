<script setup lang="ts">
import { computed, ref } from "vue";
import { useToast } from "primevue/usetoast";
import type { CartItem, CreateOrderRequest } from "../types.ts";
import { api } from "../services/api";

const props = defineProps<{ cart: CartItem[]; onClearCart: () => void }>();
const toast = useToast();
const showConfirmDialog = ref(false);

const form = ref<CreateOrderRequest>({
  customerName: "",
  email: "",
  phone: "",
  address: "",
  city: "",
  notes: "",
  items: [],
});
const sending = ref(false);

const total = computed(() =>
  props.cart.reduce((sum, item) => sum + item.product.price * item.quantity, 0),
);

const confirmOrder = () => {
  if (props.cart.length === 0) {
    toast.add({
      severity: "warn",
      summary: "Panier vide",
      detail: "Ajoutez des produits avant de commander",
      life: 3000,
    });
    return;
  }
  showConfirmDialog.value = true;
};

const submit = async () => {
  showConfirmDialog.value = false;
  sending.value = true;

  form.value.items = props.cart.map((item) => ({
    productId: item.product.id,
    quantity: item.quantity,
  }));

  try {
    const order = await api.createOrder(form.value);
    toast.add({
      severity: "success",
      summary: "Commande validée",
      detail: `Numéro de commande: #${order.id}`,
      life: 4000,
    });
    props.onClearCart();
  } catch (err) {
    console.error(err);
    toast.add({
      severity: "error",
      summary: "Erreur",
      detail: "Erreur lors de la commande. Vérifiez vos données.",
      life: 3000,
    });
  } finally {
    sending.value = false;
  }
};
</script>

<template>
  <section id="checkout" class="section">
    <Toast />
    <div class="container">
      <div class="section-head">
        <div>
          <div class="eyebrow">Checkout</div>
          <h2 class="section-title">Finalize Your Order</h2>
        </div>
        <div class="tag">Total: {{ total }} DT</div>
      </div>

      <div class="checkout-grid">
        <div class="card">
          <h3>Votre panier</h3>
          <Message v-if="cart.length === 0" severity="warn">
            Panier vide. Aucun article à commander.
          </Message>
          <div v-else class="cart-list">
            <div v-for="item in cart" :key="item.product.id" class="cart-item">
              <div>
                <strong>{{ item.product.name }}</strong>
                <p>{{ item.quantity }} x {{ item.product.price }} DT</p>
              </div>
              <span>{{ item.product.price * item.quantity }} DT</span>
            </div>
          </div>
        </div>

        <form class="card">
          <InputText
            v-model="form.customerName"
            placeholder="Nom complet"
            required
          />
          <InputText
            v-model="form.email"
            type="email"
            placeholder="Email"
            required
          />
          <InputText v-model="form.phone" placeholder="Téléphone" required />
          <InputText v-model="form.address" placeholder="Adresse" required />
          <InputText v-model="form.city" placeholder="Ville" required />
          <Textarea v-model="form.notes" placeholder="Notes (optionnel)" />
          <Button
            label="Valider la commande"
            severity="primary"
            type="button"
            :loading="sending"
            @click="confirmOrder"
          />
        </form>
      </div>

      <Dialog
        v-model:visible="showConfirmDialog"
        modal
        header="Confirmer la commande"
        :style="{ width: '90vw', maxWidth: '500px' }"
      >
        <div>
          <p>
            Êtes-vous sûr de vouloir valider cette commande pour un montant
            total de
            <strong>{{ total }} DT</strong> ?
          </p>
          <p style="color: var(--muted); font-size: 0.9rem">
            Un email de confirmation sera envoyé à
            <strong>{{ form.email }}</strong>
          </p>
        </div>

        <template #footer>
          <Button
            label="Annuler"
            severity="secondary"
            @click="showConfirmDialog = false"
          />
          <Button
            label="Confirmer la commande"
            severity="primary"
            :loading="sending"
            @click="submit"
          />
        </template>
      </Dialog>
    </div>
  </section>
</template>
