import api from './api';

/**
 * Create a new sale
 * @param {Object} saleData - Sale data with customer ID and sale details
 * @returns {Promise} Response with created sale
 */
export const createSale = async (saleData) => {
    const response = await api.post('/sales', saleData);
    return response.data;
};

/**
 * Get sales by customer ID
 * @param {number} customerId - Customer ID
 * @returns {Promise} Response with sales array
 */
export const getSalesByCustomer = async (customerId) => {
    const response = await api.get(`/sales/customer/${customerId}`);
    return response.data;
};

/**
 * Get a single sale by ID
 * @param {number} id - Sale ID
 * @returns {Promise} Response with sale details
 */
export const getSale = async (id) => {
    const response = await api.get(`/sales/${id}`);
    return response.data;
};
