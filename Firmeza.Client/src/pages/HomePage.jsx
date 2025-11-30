import { Link } from 'react-router-dom';
import './HomePage.css';

const HomePage = () => {
    return (
        <div className="home-page">
            <section className="hero-section">
                <div className="container">
                    <div className="hero-content fade-in">
                        <h1 className="hero-title">
                            Welcome to <span className="gradient-text">Firmeza</span>
                        </h1>
                        <p className="hero-subtitle">
                            Your one-stop shop for quality products at amazing prices
                        </p>
                        <div className="hero-actions">
                            <Link to="/products" className="btn btn-primary btn-lg">
                                Browse Products
                            </Link>
                            <Link to="/register" className="btn btn-outline btn-lg">
                                Get Started
                            </Link>
                        </div>
                    </div>
                </div>
            </section>

            <section className="features-section">
                <div className="container">
                    <h2 className="text-center mb-xl">Why Choose Firmeza?</h2>
                    <div className="grid grid-3">
                        <div className="feature-card card fade-in">
                            <div className="feature-icon">🚀</div>
                            <h3>Fast Delivery</h3>
                            <p>Get your products delivered quickly and safely to your doorstep.</p>
                        </div>
                        <div className="feature-card card fade-in" style={{ animationDelay: '0.1s' }}>
                            <div className="feature-icon">💳</div>
                            <h3>Secure Payment</h3>
                            <p>Your transactions are protected with industry-standard security.</p>
                        </div>
                        <div className="feature-card card fade-in" style={{ animationDelay: '0.2s' }}>
                            <div className="feature-icon">📧</div>
                            <h3>Email Receipts</h3>
                            <p>Receive detailed receipts directly to your email for every purchase.</p>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    );
};

export default HomePage;
