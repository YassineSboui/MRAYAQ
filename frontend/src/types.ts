export type Product = {
    name: string
    price: string
    tag: string
    desc: string
}

export type CultureCard = {
    title: string
    text: string
}

export type Quote = {
    text: string
    author: string
}

export type ContactForm = {
    name: string
    email: string
    phone: string
    message: string
}

export type CategoryDto = {
    id: string
    name: string
    slug: string
}

export type CreateCategoryRequest = {
    name: string
    slug: string
}

export type ProductDto = {
    id: string
    name: string
    description: string
    price: number
    tag: string
    imageUrl: string
    isFeatured: boolean
    categoryId: string
    categoryName: string
}

export type CreateProductRequest = {
    name: string
    description: string
    price: number
    tag: string
    imageUrl: string
    isFeatured: boolean
    categoryId: string
}

export type LoginRequest = {
    username: string
    password: string
}

export type LoginResponse = {
    token: string
    username: string
}

export type CartItem = {
    product: ProductDto
    quantity: number
}

export type CreateOrderItemRequest = {
    productId: string
    quantity: number
}

export type CreateOrderRequest = {
    customerName: string
    email: string
    phone: string
    address: string
    city: string
    notes: string
    items: CreateOrderItemRequest[]
}

export type OrderItemDto = {
    productId: string
    productName: string
    quantity: number
    unitPrice: number
}

export type OrderDto = {
    id: string
    customerName: string
    email: string
    phone: string
    address: string
    city: string
    notes: string
    totalAmount: number
    status: string
    createdAt: string
    items: OrderItemDto[]
}
