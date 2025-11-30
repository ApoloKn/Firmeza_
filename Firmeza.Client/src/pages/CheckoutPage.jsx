import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useCart } from '../contexts/CartContext';
import { createSale } from '../api/salesApi';
import './CheckoutPage.css';

const CheckoutPage = () => {
    const { cart, getSubtotal, getTax, getTotal, clearCart } = useCart();
    const [paymentMethod, setPaymentMethod] = useState('Credit Card');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleCheckout = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        try {
            // For this demo, we'll use customer ID 1 (the seeded customer)
            // In a real app, you'd get this from the user profile
            const saleData = {
                customerId: 1,
                subTotal: getSubtotal(),
                tax: getTax(),
                discount: 0,
                total: getTotal(),
                paymentMethod: paymentMethod,
                status: 'Completed',
                saleDetails: cart.map(item => ({
                    productId: item.id,
                    quantity: item.quantity,
                    unitPrice: item.price,
                    discount: 0,
                    subTotal: item.price * item.quantity
                }))
            };

            const sale = await createSale(saleData);

            clearCart();
            navigate(`/order-confirmation/${sale.id}`);
        } catch (err) {
            setError('Failed to process order. Please try again.');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    if (cart.length === 0) {
        navigate('/cart');
        return null;
    }

    return (
        <div className="checkout-page">
            <div className="container">
                <h1>Checkout</h1>

                {error && (
                    <div className="alert alert-error">
                        {error}
                    </div>
                )}

                <div className="checkout-layout">
                    <div className="checkout-form">
                        <form onSubmit={handleCheckout}>
                            <div className="form-section card">
                                <h2>Payment Method</h2>

                                <div className="payment-methods">
                                    {['Credit Card', 'Debit Card', 'PayPal', 'Cash'].map(method => (
                                        <label key={method} className="payment-option">
                                            <input
                                                type="radio"
                                                name="paymentMethod"
                                                value={method}
                                                checked={paymentMethod === method}
                                                onChange={(e) => setPaymentMethod(e.target.value)}
                                            />
                                            <span>{method}</span>
                                        </label>
                                    ))}
                                </div>
                            </div>

                            <button
                                type="submit"
                                className="btn btn-primary btn-lg"
                                style={{ width: '100%' }}
                                disabled={loading}
                            >
                                {loading ? <div className="spinner"></div> : `Place Order - $${getTotal().toFixed(2)}`}
                            </button>
                        </form>
                    </div>

                    <div className="order-summary card card-glass">
                        <h2>Order Summary</h2>

                        <div className="order-items">
                            {cart.map((item) => (
                                <div key={item.id} className="order-item">
                                    <div className="order-item-name">
                                        {item.name} <span>× {item.quantity}</span>
                                    </div>
                                    <div className="order-item-price">
                                        ${(item.price * item.quantity).toFixed(2)}
                                    </div>
                                </div>
                            ))}
                        </div>

                        <div className="summary-divider"></div>

                        <div className="summary-row">
                            <span>Subtotal:</span>
                            <span>${getSubtotal().toFixed(2)}</span>
                        </div>

                        <div className="summary-row">
                            <span>Tax (10%):</span>
                            <span>${getTax().toFixed(2)}</span>
                        </div>

                        <div className="summary-divider"></div>

                        <div className="summary-row summary-total">
                            <span>Total:</span>
                            <span>${getTotal().toFixed(2)}</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CheckoutPage;
