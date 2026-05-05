<script setup lang="ts">
import { ref, onMounted, onUnmounted } from "vue";
import { RouterLink } from "vue-router";

interface Banner {
  id: string;
  title: string;
  subtitle?: string;
  image?: string;
  cta?: { label: string; link: string };
}

const banners = ref<Banner[]>([
  {
    id: "1",
    title: "Nouvelle Collection",
    subtitle: "Styles Urbains Pour GenZ",
    image: "/images/banner-nouvelle-collection.png",
    cta: { label: "Découvrir", link: "/catalog" },
  },
  {
    id: "2",
    title: "Drop Exclusif",
    subtitle: "Pièces Limitées à Saisir",
    image: "/images/banner-drop-exclusif.png",
    cta: { label: "Commander", link: "/catalog" },
  },
  {
    id: "3",
    title: "Street Culture Tunisienne",
    subtitle: "Chwada Local, Style Mondial",
    image: "/images/banner-street-culture.png",
    cta: { label: "Explorez", link: "/lookbook" },
  },
  {
    id: "4",
    title: "Collab Artistes Locaux",
    subtitle: "Édition Limitée Tunisienne",
    image: "/images/banner-collab-artistes.png",
    cta: { label: "Boutique", link: "/catalog" },
  },
]);

const currentBanner = ref(0);
let autoRotateInterval: ReturnType<typeof setInterval>;

const startAutoRotate = () => {
  autoRotateInterval = setInterval(() => {
    currentBanner.value = (currentBanner.value + 1) % banners.value.length;
  }, 5000);
};

onMounted(() => {
  startAutoRotate();
});

onUnmounted(() => {
  if (autoRotateInterval) {
    clearInterval(autoRotateInterval);
  }
});
</script>

<template>
  <div class="banner-carousel">
    <Carousel
      :value="banners"
      :numVisible="1"
      :numScroll="1"
      :autoplayInterval="5000"
      :circular="true"
      :showIndicators="true"
      containerClass="banner-carousel-container"
      contentClass="banner-carousel-content"
    >
      <template #item="{ data: banner }">
        <div class="banner-wrapper">
          <div
            v-if="banner.image"
            class="banner-with-image"
            :style="{ backgroundImage: `url(${banner.image})` }"
          >
            <div class="banner-overlay"></div>
          </div>
          <div v-else class="banner-gradient"></div>
          <div class="banner-content">
            <h1 class="banner-title">{{ banner.title }}</h1>
            <p v-if="banner.subtitle" class="banner-subtitle">
              {{ banner.subtitle }}
            </p>
            <RouterLink
              v-if="banner.cta"
              :to="banner.cta.link"
              class="banner-cta"
            >
              <Button
                :label="banner.cta.label"
                severity="primary"
                size="large"
              />
            </RouterLink>
          </div>
        </div>
      </template>
    </Carousel>
  </div>
</template>

<style scoped>
.banner-carousel {
  width: 100%;
  margin: 0 0 20px 0;
}

:deep(.p-carousel) {
  background: transparent;
}

:deep(.p-carousel-content) {
  gap: 0;
  padding: 0;
}

:deep(.p-carousel-viewport) {
  border-radius: var(--radius-md);
  overflow: hidden;
}

.banner-carousel-container {
  border-radius: var(--radius-md);
  overflow: hidden;
}

.banner-carousel-content {
  padding: 0;
}

.banner-wrapper {
  position: relative;
  height: 500px;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
}

.banner-with-image {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
}

.banner-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(
    135deg,
    rgba(15, 28, 26, 0.7) 0%,
    rgba(47, 123, 94, 0.5) 100%
  );
  z-index: 1;
}

.banner-gradient {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #c99b47 0%, #2f7b5e 100%);
  z-index: 1;
}

.banner-content {
  position: relative;
  z-index: 2;
  text-align: center;
  width: 100%;
  max-width: 600px;
  padding: 40px;
}

.banner-title {
  font-size: clamp(2.5rem, 8vw, 4rem);
  font-family: "Cinzel", serif;
  text-transform: uppercase;
  letter-spacing: 0.15em;
  margin: 0 0 16px 0;
  color: var(--gold-soft);
  text-shadow: 2px 2px 8px rgba(0, 0, 0, 0.5);
}

.banner-subtitle {
  font-size: clamp(1rem, 3vw, 1.4rem);
  color: var(--cream);
  margin: 0 0 24px 0;
  font-weight: 300;
  letter-spacing: 0.05em;
  text-shadow: 1px 1px 4px rgba(0, 0, 0, 0.5);
}

.banner-cta {
  display: inline-block;
  text-decoration: none;
}

/* PrimeVue Carousel Navigation Styling */
:deep(.p-carousel-prev),
:deep(.p-carousel-next) {
  background: rgba(201, 155, 71, 0.2) !important;
  border: 2px solid var(--gold) !important;
  color: var(--gold) !important;
  width: 50px !important;
  height: 50px !important;
  transition: all 0.3s ease !important;
}

:deep(.p-carousel-prev:hover),
:deep(.p-carousel-next:hover) {
  background: rgba(201, 155, 71, 0.4) !important;
  transform: scale(1.1) !important;
}

:deep(.p-carousel-indicators .p-carousel-indicator) {
  background: rgba(255, 255, 255, 0.3) !important;
  border: 2px solid rgba(255, 255, 255, 0.5) !important;
  width: 12px !important;
  height: 12px !important;
}

:deep(.p-carousel-indicators .p-carousel-indicator.p-highlight) {
  background: var(--gold) !important;
  border-color: var(--gold) !important;
}

/* Mobile */
@media (max-width: 768px) {
  .banner-wrapper {
    height: 350px;
  }

  .banner-title {
    font-size: 1.8rem;
  }

  .banner-subtitle {
    font-size: 1rem;
  }

  :deep(.p-carousel-prev),
  :deep(.p-carousel-next) {
    width: 40px !important;
    height: 40px !important;
  }
}

@media (max-width: 480px) {
  .banner-wrapper {
    height: 280px;
  }

  .banner-title {
    font-size: 1.5rem;
  }

  .banner-subtitle {
    font-size: 0.9rem;
  }

  .banner-content {
    padding: 24px;
  }

  :deep(.p-carousel-prev),
  :deep(.p-carousel-next) {
    width: 36px !important;
    height: 36px !important;
  }
}
</style>
