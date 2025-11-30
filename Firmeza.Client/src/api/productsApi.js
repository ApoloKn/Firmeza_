import api from './api';

/**
 * Get all products with pagination
 * @param {number} page - Page number
 * @param {number} pageSize - Number of items per page
 * @returns {Promise} Response with products array
 */
export const getProducts = async (page = 1, pageSize = 12) => {
    const response = await api.get('/products', { params: { page, pageSize } });
    return response.data;
};

/**
 * Get a single product by ID
 * @param {number} id - Product ID
 * @returns {Promise} Response with product details
 */
export const getProduct = async (id) => {
    const response = await api.get(`/products/${id}`);
    return response.data;
};
