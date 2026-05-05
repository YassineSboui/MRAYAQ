import { reactive } from 'vue'
import type { CartItem, ProductDto } from '../types.ts'

const state = reactive<{ items: CartItem[] }>({ items: [] })

export const useCart = () => {
    const add = (product: ProductDto) => {
        const existing = state.items.find(entry => entry.product.id === product.id)
        if (existing) {
            existing.quantity += 1
        } else {
            state.items.push({ product, quantity: 1 })
        }
    }

    const clear = () => {
        state.items.length = 0
    }

    return {
        items: state.items,
        add,
        clear
    }
}

// Global cart instance
export const cart = {
    items: state.items,
    addItem: (product: ProductDto) => {
        const existing = state.items.find(entry => entry.product.id === product.id)
        if (existing) {
            existing.quantity += 1
        } else {
            state.items.push({ product, quantity: 1 })
        }
    },
    clear: () => {
        state.items.length = 0
    }
}
