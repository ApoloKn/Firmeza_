import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { getSale } from '../api/salesApi';
import { sendReceipt } from '../api/emailApi';
import { FiMail, FiCheck, FiPackage } from 'react-icons/fi';
import './OrderConfirmationPage.css';

const OrderConfirmationPage = () => {
    const { id } = useParams();
    const [sale, setSale] = useState(null);
    const [loading, setLoading] = useState(true);
    const [sendingEmail, setSendingEmail] = useState(false);
    const [emailSent, setEmailSent] = useState(false);
    const [emailError, setEmailError] = useState('');

    useEffect(() => {
        loadSale();
    }, [id]);

    const loadSale = async () => {
        try {
            const data = await getSale(id);
            setSale(data);
        } catch (err) {
            console.error('Failed to load sale', err);
        } finally {
            setLoading(false);
        }
    };

    const handleSendReceipt = async () => {
        setSendingEmail(true);
        setEmailError('');

        try {
            await sendReceipt(id);
            setEmailSent(true);
        } catch (err) {
            setEmailError('Failed to send receipt. Please try again.');
            console.error(err);
        } finally {
            setSendingEmail(false);
        }
    };

    if (loading) {
        return (
            <div className="flex flex-center" style={{ minHeight: '50vh' }}>
                <div className="spinner-lg"></div>
            </div>
        );
    }

    if (!sale) {
        return (
            <div className="container" style={{ padding: '4rem 0', textAlign: 'center' }}>
                <h2>Order not found</h2>
                <Link to="/products" className="btn btn-primary">
                    Continue Shopping
                </Link>
            </div>
        );
    }

    return (
        <div className="order-confirmation-page">
            <div className="container">
                <div className="confirmation-card card fade-in">
                    <div className="confirmation-icon">
                        <FiCheck size={60} />
                    </div>

                    <h1>Order Confirmed!</h1>
                    <p className="confirmation-subtitle">
                        Thank you for your purchase. Your order has been successfully placed.
                    </p>

                    <div className="order-number">
                        <FiPackage />
                        <span>Order #{sale.id}</span>
                    </div>

                    <div className="order-details card-glass">
                        <h2>Order Details</h2>

                        <div className="detail-row">
                            <span>Date:</span>
                            <span>{new Date(sale.saleDate).toLocaleDateString()}</span>
                        </div>

                        <div className="detail-row">
                            <span>Payment Method:</span>
                            <span>{sale.paymentMethod}</span>
                        </div>

                        <div className="detail-row">
                            <span>Status:</span>
                            <span className="badge">{sale.status}</span>
                        </div>

                        <div className="order-items-section">
                            <h3>Items</h3>
                            {sale.saleDetails?.map((detail, index) => (
                                <div key={index} className="order-item-row">
                                    <span>{detail.productName || `Product #${detail.productId}`} × {detail.quantity}</span>
                                    <span>${detail.subTotal.toFixed(2)}</span>
                                </div>
                            ))}
                        </div>

                        <div className="summary-divider"></div>

                        <div className="detail-row">
                            <span>Subtotal:</span>
                            <span>${sale.subTotal.toFixed(2)}</span>
                        </div>

                        <div className="detail-row">
                            <span>Tax:</span>
                            <span>${sale.tax.toFixed(2)}</span>
                        </div>

                        {sale.discount > 0 && (
                            <div className="detail-row">
                                <span>Discount:</span>
                                <span>-${sale.discount.toFixed(2)}</span>
                            </div>
                        )}

                        <div className="detail-row total-row">
                            <span>Total:</span>
                            <span>${sale.total.toFixed(2)}</span>
                        </div>
                    </div>

                    <div className="email-section">
                        {emailSent ? (
                            <div className="alert alert-success">
                                <FiCheck /> Receipt sent successfully to your email!
                            </div>
                        ) : (
                            <>
                                {emailError && (
                                    <div className="alert alert-error">
                                        {emailError}
                                    </div>
                                )}
                                <button
                                    onClick={handleSendReceipt}
                                    className="btn btn-secondary btn-lg"
                                    disabled={sendingEmail}
                                >
                                    {sendingEmail ? (
                                        <div className="spinner"></div>
                                    ) : (
                                        <>
                                            <FiMail /> Send Receipt to Email
                                        </>
                                    )}
                                </button>
                            </>
                        )}
                    </div>

                    <Link to="/products" className="btn btn-primary btn-lg">
                        Continue Shopping
                    </Link>
                </div>
            </div>
        </div>
    );
};

export default OrderConfirmationPage;
