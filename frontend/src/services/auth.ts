const tokenKey = 'mrayaq_admin_token'

export const auth = {
    getToken(): string | null {
        return localStorage.getItem(tokenKey)
    },
    setToken(token: string) {
        localStorage.setItem(tokenKey, token)
    },
    clearToken() {
        localStorage.removeItem(tokenKey)
    },
    isLoggedIn(): boolean {
        return !!localStorage.getItem(tokenKey)
    }
}
