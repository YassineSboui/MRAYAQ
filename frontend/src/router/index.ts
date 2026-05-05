import { createRouter, createWebHistory } from 'vue-router'

import HomeView from '../pages/HomeView.vue'
import CollectionsView from '../pages/CollectionsView.vue'
import CatalogView from '../pages/CatalogView.vue'
import CultureView from '../pages/CultureView.vue'
import LookbookView from '../pages/LookbookView.vue'
import CheckoutView from '../pages/CheckoutView.vue'
import AdminView from '../pages/AdminView.vue'
import ContactView from '../pages/ContactView.vue'

const routes = [
    { path: '/', name: 'home', component: HomeView },
    { path: '/collections', name: 'collections', component: CollectionsView },
    { path: '/catalog', name: 'catalog', component: CatalogView },
    { path: '/culture', name: 'culture', component: CultureView },
    { path: '/lookbook', name: 'lookbook', component: LookbookView },
    { path: '/checkout', name: 'checkout', component: CheckoutView },
    { path: '/admin', name: 'admin', component: AdminView },
    { path: '/contact', name: 'contact', component: ContactView }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
    scrollBehavior() {
        return { top: 0 }
    }
})

export default router
