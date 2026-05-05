import type {
    CategoryDto,
    CreateCategoryRequest,
    CreateOrderRequest,
    CreateProductRequest,
    LoginRequest,
    LoginResponse,
    OrderDto,
    ProductDto
} from '../types.ts'
import { auth } from './auth'

const baseUrl = import.meta.env.VITE_API_BASE ?? 'https://localhost:5001'

const toQuery = (params: Record<string, string | number | undefined>) => {
    const search = new URLSearchParams()
    Object.entries(params).forEach(([key, value]) => {
        if (value !== undefined && value !== '') {
            search.append(key, String(value))
        }
    })
    return search.toString()
}

const adminHeaders = () => {
    const token = auth.getToken()
    if (!token) {
        throw new Error('Admin not authenticated')
    }
    return {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`
    }
}

export const api = {
    async login(payload: LoginRequest): Promise<LoginResponse> {
        const response = await fetch(`${baseUrl}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        })
        return response.json()
    },
    async getCategories(): Promise<CategoryDto[]> {
        const response = await fetch(`${baseUrl}/api/categories`)
        return response.json()
    },
    async createCategory(payload: CreateCategoryRequest): Promise<CategoryDto> {
        const response = await fetch(`${baseUrl}/api/categories`, {
            method: 'POST',
            headers: adminHeaders(),
            body: JSON.stringify(payload)
        })
        return response.json()
    },
    async updateCategory(id: string, payload: CreateCategoryRequest): Promise<CategoryDto> {
        const response = await fetch(`${baseUrl}/api/categories/${id}`, {
            method: 'PUT',
            headers: adminHeaders(),
            body: JSON.stringify(payload)
        })
        return response.json()
    },
    async deleteCategory(id: string): Promise<boolean> {
        const response = await fetch(`${baseUrl}/api/categories/${id}`, {
            method: 'DELETE',
            headers: adminHeaders()
        })
        return response.ok
    },
    async getProducts(filters: {
        search?: string
        categoryId?: string
        minPrice?: number
        maxPrice?: number
    }): Promise<ProductDto[]> {
        const query = toQuery({
            search: filters.search,
            categoryId: filters.categoryId,
            minPrice: filters.minPrice,
            maxPrice: filters.maxPrice
        })
        const url = query ? `${baseUrl}/api/products?${query}` : `${baseUrl}/api/products`
        const response = await fetch(url)
        return response.json()
    },
    async createProduct(payload: CreateProductRequest): Promise<ProductDto> {
        const response = await fetch(`${baseUrl}/api/products`, {
            method: 'POST',
            headers: adminHeaders(),
            body: JSON.stringify(payload)
        })
        return response.json()
    },
    async updateProduct(id: string, payload: CreateProductRequest): Promise<ProductDto> {
        const response = await fetch(`${baseUrl}/api/products/${id}`, {
            method: 'PUT',
            headers: adminHeaders(),
            body: JSON.stringify(payload)
        })
        return response.json()
    },
    async deleteProduct(id: string): Promise<boolean> {
        const response = await fetch(`${baseUrl}/api/products/${id}`, {
            method: 'DELETE',
            headers: adminHeaders()
        })
        return response.ok
    },
    async createOrder(payload: CreateOrderRequest): Promise<OrderDto> {
        const response = await fetch(`${baseUrl}/api/orders`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        })
        return response.json()
    },
    async getOrders(): Promise<OrderDto[]> {
        const response = await fetch(`${baseUrl}/api/orders`, {
            method: 'GET',
            headers: adminHeaders()
        })
        return response.json()
    },
    async updateOrderStatus(id: string, status: string): Promise<OrderDto> {
        const url = `${baseUrl}/api/orders/${id}/status`
        const body = JSON.stringify({ status })
        const headers = adminHeaders()

        console.log('Update Order Status Request:', {
            url,
            method: 'PUT',
            headers,
            body
        })

        const response = await fetch(url, {
            method: 'PUT',
            headers,
            body
        })

        console.log('Update Order Status Response:', {
            status: response.status,
            statusText: response.statusText,
            headers: Object.fromEntries(response.headers.entries())
        })

        if (!response.ok) {
            const errorText = await response.text()
            console.error(`Status update failed: ${response.status}`, errorText)
            throw new Error(`Failed to update order status: ${response.status} ${errorText}`)
        }
        return response.json()
    }
}
