import api from './api';

/**
 * Send receipt email for a sale
 * @param {number} saleId - Sale ID
 * @returns {Promise} Response with success message
 */
export const sendReceipt = async (saleId) => {
    const response = await api.post(`/email/send-receipt/${saleId}`);
    return response.data;
};
